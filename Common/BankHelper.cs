using BankApi.dal.Models.Enums;

namespace BankApi.Common;

public static class BankHelper
{
    /// <summary>
    /// Генерирует уникальный 20-значный номер счёта на основе GUID.
    /// </summary>
    public static string GenerateAccountNumber()
    {
        var bytes = Guid.NewGuid().ToByteArray();
        ulong number = BitConverter.ToUInt64(bytes, 0);
        return number.ToString("D20");
    }

    /// <summary>
    /// Генерирует 16-значный номер карты с префиксом платёжной системы.
    /// Visa начинается с 4, Mastercard с 5, остальные с 6.
    /// Последняя цифра — контрольная сумма по алгоритму Луна.
    /// </summary>
    public static string GenerateCardNumber(PaymentSystem paymentSystem)
    {
        string prefix = paymentSystem switch
        {
            PaymentSystem.Visa => "4",
            PaymentSystem.Mastercard => "5",
            _ => "6"
        };

        var bytes = Guid.NewGuid().ToByteArray();
        ulong number = BitConverter.ToUInt64(bytes, 0);
        ulong fourteenDigitNumber = number % 100000000000000;
        string partial = prefix + fourteenDigitNumber.ToString("D14");

        return ApplyLuhn(partial);
    }

    /// <summary>
    /// Вычисляет и добавляет контрольную цифру по алгоритму Луна.
    /// Используется для генерации валидных номеров банковских карт.
    /// </summary>
    private static string ApplyLuhn(string partialNumber)
    {
        int sum = 0;
        bool alternate = true;
        for (int i = partialNumber.Length - 1; i >= 0; i--)
        {
            int digit = partialNumber[i] - '0';
            if (alternate)
            {
                digit *= 2;
                if (digit > 9) digit -= 9;
            }
            sum += digit;
            alternate = !alternate;
        }
        int checkDigit = (10 - (sum % 10)) % 10;
        return partialNumber + checkDigit;
    }

    /// <summary>
    /// Маскирует номер карты (PAN): показывает первые 6 и последние 4 цифры.
    /// Пример: 123456******7890
    /// </summary>
    public static string MaskPan(string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length < 10)
            return "************";

        return $"{cardNumber[..6]}******{cardNumber[^4..]}";
    }

    /// <summary>
    /// Маскирует номер паспорта: показывает первые 2 и последние 3 символа.
    /// Пример: AN***456
    /// </summary>
    public static string MaskPassportNumber(string number)
    {
        if (string.IsNullOrEmpty(number) || number.Length < 5)
            return "****";

        return number[..2] + new string('*', number.Length - 5) + number[^3..];
    }
    
}