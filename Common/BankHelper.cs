using BankApi.dal.Models.Enums;

namespace BankApi.Common;

public static class BankHelper
{
    // Генерация номера счёта
    public static string GenerateAccountNumber()
    {
        var bytes = Guid.NewGuid().ToByteArray();
        ulong number = BitConverter.ToUInt64(bytes, 0);
        return number.ToString("D20");
    }

    // Генерация номера карты с префиксом платёжной системы
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
    
        ulong fifteenDigitNumber = number % 1000000000000000; 
    
        string digits = fifteenDigitNumber.ToString("D15");

        return prefix + digits; 
    }
    // Маскировка номера карты (PAN)
    public static string MaskPan(string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length < 10)
            return "************";

        return $"{cardNumber[..6]}******{cardNumber[^4..]}";
    }

    // Маскировка номера паспорта
    public static string MaskPassportNumber(string number)
    {
        if (string.IsNullOrEmpty(number) || number.Length < 5)
            return "****";

        return number[..2] + new string('*', number.Length - 5) + number[^3..];
    }
}