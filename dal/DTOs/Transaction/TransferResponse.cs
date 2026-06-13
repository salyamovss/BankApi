namespace BankApi.dal.DTOs.Transaction;

public class TransferResponse
{
    public int TransactionId { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal? ConvertedAmount { get; set; }
    public decimal? ExchangeRate { get; set; }
    public string FromAccountNumber { get; set; } = string.Empty;
    public string ToAccountNumber { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Message { get; set; } = string.Empty;
}