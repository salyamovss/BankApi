using BankApi.dal.DTOs;
using BankApi.dal.DTOs.Transaction;
using BankApi.dal.Models;
using BankApi.dal.Models.Enums;

namespace BankApi.Mappers;

public class TransactionMapper
{
    public TransferResponse ToResponse(Transaction transaction, string message) => new()
    {
        TransactionId = transaction.Id,
        Status = transaction.Status.ToString(),
        Amount = transaction.Amount,
        Currency = transaction.Currency,
        ConvertedAmount = transaction.ConvertedAmount,
        ExchangeRate = transaction.ExchangeRate,
        FromAccountNumber = transaction.FromAccount.AccountNumber,
        ToAccountNumber = transaction.ToAccount.AccountNumber,
        Description = transaction.Description,
        CreatedAt = transaction.CreatedAt,
        Message = message
    };
    
    public Transaction ToEntity(TransferRequest request, string currency, ConversionResult conversion) => new()
    {
        FromAccountId = request.FromAccountId,
        ToAccountId = request.ToAccountId,
        Amount = request.Amount,
        Currency = currency,
        ConvertedAmount = conversion.IsSameCurrency ? null : conversion.ConvertedAmount,
        ExchangeRate = conversion.ExchangeRate,
        Description = request.Description,
        Status = TransactionStatus.Success,
        CreatedAt = DateTime.UtcNow
    };
}