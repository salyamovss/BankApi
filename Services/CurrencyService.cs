using System.Net;
using BankApi.Common;
using BankApi.dal.DTOs;
using BankApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BankApi.Services;

public class CurrencyService(AppDbContext db, IMemoryCache cache)
{
    private const string CacheKey = "ExchangeRates_Cache";

    public async Task<ConversionResult> Convert(decimal amount, string fromCurrency, string toCurrency)
    {
        var from = fromCurrency.ToUpper().Trim();
        var to = toCurrency.ToUpper().Trim();

        if (from == to)
            return new ConversionResult(amount, null, true);

        var rates = await GetActualRatesAsync();

        if (!rates.TryGetValue(from, out decimal rateFrom) || !rates.TryGetValue(to, out decimal rateTo))
            throw new AppException(ErrorCodes.UnsupportedCurrencyConversion, HttpStatusCode.UnprocessableEntity);

        decimal convertedAmount = Math.Round((amount * rateFrom) / rateTo, 2, MidpointRounding.AwayFromZero);
        decimal exchangeRate = Math.Round(convertedAmount / amount, 6, MidpointRounding.AwayFromZero);

        return new ConversionResult(convertedAmount, exchangeRate, false);
    }

    private async Task<Dictionary<string, decimal>> GetActualRatesAsync()
    {
        if (cache.TryGetValue(CacheKey, out Dictionary<string, decimal>? cachedRates) && cachedRates != null)
        {
            return cachedRates;
        }

        var ratesResult = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);

        try
        {
            var dbRates = await db.ExchangeRates.AsNoTracking().ToListAsync();

            if (dbRates != null && dbRates.Count > 0)
            {
                foreach (var r in dbRates)
                {
                    ratesResult[r.FromCurrency] = r.Rate;
                }
            }
            else
            {
                throw new Exception("Таблица ExchangeRates пуста в СУБД.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[CURRENCY SERVICE CRITICAL] Ошибка чтения курсов: {ex.Message}");
            throw new AppException(ErrorCodes.UnsupportedCurrencyConversion, HttpStatusCode.ServiceUnavailable);
        }

        cache.Set(CacheKey, ratesResult, TimeSpan.FromMinutes(5));
        return ratesResult;
    }
}