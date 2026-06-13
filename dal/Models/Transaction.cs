using BankApi.dal.Models;
using BankApi.dal.Models.Enums;

namespace BankApi.dal.Models;

public class Transaction
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal? ConvertedAmount { get; set; }
    public decimal? ExchangeRate { get; set; }
    public string? Description { get; set; }
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int FromAccountId { get; set; }
    public Account FromAccount { get; set; } = null!;

    public int ToAccountId { get; set; }
    public Account ToAccount { get; set; } = null!;
}