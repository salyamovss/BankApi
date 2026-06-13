namespace BankApi.dal.DTOs.Card;

public class CardResponseMessage
{
    public string Message { get; set; } = string.Empty;
    public CardResponse Card { get; set; } = null!;
}