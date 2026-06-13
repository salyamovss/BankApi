using BankApi.dal.DTOs.Account;
using BankApi.dal.DTOs.Card;
using BankApi.dal.Models;
using BankApi.dal.Models.Enums;

namespace BankApi.Mappers;

public class AccountMapper
{
    public AccountResponse ToResponse(Account account) => new()
    {
        Id = account.Id,
        AccountNumber = account.AccountNumber,
        Currency = account.Currency.ToString(),
        Balance = account.Balance,
        Type = account.Type.ToString(), 
        Status = account.Status.ToString(),
        CreatedAt = account.CreatedAt,
        UserId = account.UserId
    };

    public Account ToEntity(CreateAccountRequest request, int userId, string generatedAccountNumber) => new()
    {
        AccountNumber = generatedAccountNumber,
        Currency = request.Currency,
        Balance = 0, 
        Status = AccountStatus.Active,
        CreatedAt = DateTime.UtcNow,
        UserId = userId
    };
    
    public CloseAccountResponse ToCloseResponse(
        Account accountToClose, 
        string message, 
        Account? targetAccount = null)
    {
        return new CloseAccountResponse
        {
            AccountId = accountToClose.Id,
            AccountNumber = accountToClose.AccountNumber,
            Message = message,
            TargetAccountId = targetAccount?.Id, // если null, запишется null
            TargetAccountNumber = targetAccount?.AccountNumber, // если null, запишется null
            ClosedCards = accountToClose.Cards.Select(c => new ClosedCardDto
            {
                CardholderName = c.HolderName,
                MaskedPan = MaskPan(c.CardNumber) 
            }).ToList()
        };
    }
    
    private static string MaskPan(string pan)
    {
        if (string.IsNullOrWhiteSpace(pan) || pan.Length < 16)
            return pan; 
        return $"{pan[..6]}******{pan[^4..]}";
    }
    
}