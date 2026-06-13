namespace BankApi.dal.DTOs.Card;

public class CardResponse
{
    public int Id { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public string Product { get; set; } = string.Empty;
    public string PaymentSystem { get; set; } = string.Empty;
    public string HolderName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateOnly ExpiryDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public int AccountId { get; set; }
}