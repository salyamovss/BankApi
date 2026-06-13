using System.Globalization;
using System.Xml.Linq;
using BankApi.dal.Models;
using BankApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Services.job;

public class CurrencyUpdateJob(IServiceProvider serviceProvider, ILogger<CurrencyUpdateJob> logger) : BackgroundService
{
    // Официальный XML фид Национального Банка Кыргызской Республики
    private const string NbkrApiUrl = "https://www.nbkr.kg/XML/daily.xml";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Джoба обновления курсов валют (НБКР) запущена.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await UpdateRatesFromNbkrAsync();
            }
            catch (Exception ex)
            {
                logger.LogError($"[NBKR JOB ERROR] Не удалось обновить курсы от НБКР: {ex.Message}");
            }
            
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }

private async Task UpdateRatesFromNbkrAsync()
{
    using var httpClient = new HttpClient();
    
    var response = await httpClient.GetAsync(NbkrApiUrl);
    if (!response.IsSuccessStatusCode)
    {
        logger.LogWarning($"Сайт НБКР вернул ошибку: {response.StatusCode}");
        return;
    }

    string xmlString = await response.Content.ReadAsStringAsync();
    var xDoc = XDocument.Parse(xmlString);
    var currencyElements = xDoc.Descendants("Currency").ToList();
    
    if (!currencyElements.Any())
    {
        logger.LogWarning("В XML от НБКР не найдены элементы <Currency>.");
        return;
    }

    using var scope = serviceProvider.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    var dbRates = await db.ExchangeRates.ToListAsync();
    var targetCurrencies = new[] { "USD", "EUR", "RUB" };
    var kgCulture = new CultureInfo("ru-RU");

    foreach (var xmlCurrency in currencyElements)
    {
        string? isoCode = xmlCurrency.Attribute("ISOCode")?.Value?.ToUpper();
        
        if (isoCode != null && targetCurrencies.Contains(isoCode))
        {
            string? valueStr = xmlCurrency.Element("Value")?.Value;
            string? nominalStr = xmlCurrency.Element("Nominal")?.Value;

            if (decimal.TryParse(valueStr, NumberStyles.Any, kgCulture, out decimal rate) &&
                decimal.TryParse(nominalStr, NumberStyles.Any, kgCulture, out decimal nominal))
            {
                decimal finalRate = rate / nominal;
                finalRate = Math.Round(finalRate, 4, MidpointRounding.AwayFromZero);

                var existingRate = dbRates.FirstOrDefault(r => r.FromCurrency == isoCode);
                
                if (existingRate != null)
                {
                    existingRate.Rate = finalRate;
                    existingRate.UpdatedAt = DateTime.UtcNow;
                    logger.LogInformation($"[JOB UPDATE] Валюта {isoCode} обновлена: {finalRate} KGS");
                }
                else
                {
                    var newRate = new ExchangeRate
                    {
                        FromCurrency = isoCode,
                        ToCurrency = "KGS",
                        Rate = finalRate,
                        UpdatedAt = DateTime.UtcNow
                    };
                    db.ExchangeRates.Add(newRate);
                    logger.LogInformation($"[JOB INSERT] Валюта {isoCode} впервые добавлена в БД: {finalRate} KGS");
                }
            }
        }
    }

    var kgsRate = dbRates.FirstOrDefault(r => r.FromCurrency == "KGS");
    if (kgsRate == null)
    {
        db.ExchangeRates.Add(new ExchangeRate
        {
            FromCurrency = "KGS",
            ToCurrency = "KGS",
            Rate = 1.0m,
            UpdatedAt = DateTime.UtcNow
        });
        logger.LogInformation("[JOB INSERT] Базовая валюта KGS успешно добавлена в БД.");
    }
    else
    {
        kgsRate.UpdatedAt = DateTime.UtcNow;
    }

    await db.SaveChangesAsync();
    logger.LogInformation("База данных успешно синхронизирована с официальными курсами НБКР.");
}
}