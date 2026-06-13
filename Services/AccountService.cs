using System.Net;
using BankApi.Common;
using BankApi.Data;
using BankApi.Mappers; 
using BankApi.dal.Models;
using BankApi.dal.DTOs.Account;
using BankApi.dal.Models.Enums;
using BankApi.dal.Repositories;

namespace BankApi.Services;

public class AccountService(AppDbContext db, IAccountRepository accountRepository, AccountMapper mapper, CurrencyService currencyService)
{
    
 public async Task<AccountResponse> Create(int userId, CreateAccountRequest request)
{
    var user = await db.Users.FindAsync(userId)
               ?? throw new AppException(ErrorCodes.UserNotFound, HttpStatusCode.NotFound);

    if (!user.IsActive)
        throw new AppException(ErrorCodes.UserNotFound, HttpStatusCode.NotFound);

    string accountNumber = BankHelper.GenerateAccountNumber();
    var account = mapper.ToEntity(request, userId, accountNumber);
    account.Currency = request.Currency.ToUpper().Trim();
    account.Type = request.Type;

    db.Accounts.Add(account);
    await db.SaveChangesAsync();

    return mapper.ToResponse(account);
}

public async Task<AccountResponse> GetById(int id)
{
    var account = await db.Accounts.FindAsync(id)
        ?? throw new AppException(ErrorCodes.AccountNotFound, HttpStatusCode.NotFound);

    return mapper.ToResponse(account);
}

    public async Task<CloseAccountResponse> Close(int id, CloseAccountRequest request)
    {
        var accountToClose = await accountRepository.GetByIdWithCardsAsync(id)
            ?? throw new AppException(ErrorCodes.AccountNotFound, HttpStatusCode.NotFound);

        if (accountToClose.Status == AccountStatus.Closed)
            throw new AppException(ErrorCodes.AccountClosed, HttpStatusCode.Conflict);

        string messageText;

        if (accountToClose.Balance == 0)
        {
            ExecuteCloseActions(accountToClose);
            await db.SaveChangesAsync();
            messageText = ErrorMessages.Get(ErrorCodes.AccountClosedSuccess);
            return mapper.ToCloseResponse(accountToClose, messageText);
        }

        var otherAccounts = await accountRepository.GetActiveByUserIdExceptAsync(accountToClose.UserId, id);

        if (request.TargetAccountId == null)
        {
            if (otherAccounts.Count == 0)
                throw new AppException(ErrorCodes.AccountHasBalanceNoOtherAccounts, HttpStatusCode.UnprocessableEntity, accountToClose.Balance);

            throw new AppException(ErrorCodes.AccountHasBalanceChooseTarget, HttpStatusCode.UnprocessableEntity, accountToClose.Balance);
        }

        var targetAccount = otherAccounts.FirstOrDefault(a => a.Id == request.TargetAccountId)
            ?? throw new AppException(ErrorCodes.InvalidTargetAccountForTransfer, HttpStatusCode.UnprocessableEntity);

        decimal originalBalance = accountToClose.Balance;
        var conversion = await currencyService.Convert(originalBalance, accountToClose.Currency, targetAccount.Currency);
        targetAccount.Balance += conversion.ConvertedAmount;
        accountToClose.Balance = 0;

        ExecuteCloseActions(accountToClose);
        await db.SaveChangesAsync();

        messageText = ErrorMessages.Get(
            ErrorCodes.AccountClosedWithTransfer,
            new object[] { $"{originalBalance} {accountToClose.Currency}", targetAccount.AccountNumber }
        );
        return mapper.ToCloseResponse(accountToClose, messageText, targetAccount);
    }

    public async Task<List<AccountResponse>> GetByUserId(int userId) 
    { 
        var accounts = await accountRepository.GetByUserIdAsync(userId); 
        return accounts.Select(mapper.ToResponse).ToList(); 
    }
    
    private static void ExecuteCloseActions(Account account)
    {
        account.Status = AccountStatus.Closed;
        account.ClosedAt = DateTime.UtcNow;

        foreach (var card in account.Cards)
            card.Status = CardStatus.Blocked;
    } 
    
    public async Task<AccountResponse> Restore(int id)
    {
        var account = await accountRepository.GetByIdWithCardsAsync(id)
                      ?? throw new AppException(ErrorCodes.AccountNotFound, HttpStatusCode.NotFound);

        if (account.Status == AccountStatus.Active)
            throw new AppException(ErrorCodes.AccountAlreadyActive, HttpStatusCode.Conflict);

        account.Status = AccountStatus.Active;
        account.ClosedAt = null; 

        foreach (var card in account.Cards.Where(c => c.Status == CardStatus.Blocked))
        {
            card.Status = CardStatus.Active;
        }

        await db.SaveChangesAsync();
        return mapper.ToResponse(account);
    }
}