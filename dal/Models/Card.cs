using BankApi.dal.Models.Enums;

namespace BankApi.dal.Models;

public class Card
{
    public int Id { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public CardProduct Product { get; set; }
    public PaymentSystem PaymentSystem { get; set; }
    public string HolderName { get; set; } = string.Empty;
    public CardStatus Status { get; set; } = CardStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateOnly ExpiryDate { get; set; }

    public int AccountId { get; set; }
    public Account Account { get; set; } = null!;
    
}