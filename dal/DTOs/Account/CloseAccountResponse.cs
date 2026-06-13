using BankApi.dal.DTOs.Card;

namespace BankApi.dal.DTOs.Account;

public class CloseAccountResponse
{
    public int AccountId { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int? TargetAccountId { get; set; }
    public string? TargetAccountNumber { get; set; }
    
    public List<ClosedCardDto> ClosedCards { get; set; } = new();
}