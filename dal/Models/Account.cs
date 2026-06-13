
using BankApi.dal.Models.Enums;

namespace BankApi.dal.Models;

public class Account
{
    public int Id { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public AccountType Type { get; set; } = AccountType.Debit; 
    public string Currency { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public AccountStatus Status { get; set; } = AccountStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ClosedAt { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<Card> Cards { get; set; } = new List<Card>();
}