namespace BankApi.dal.DTOs;

public record ConversionResult(
    decimal ConvertedAmount,
    decimal? ExchangeRate,
    bool IsSameCurrency
);