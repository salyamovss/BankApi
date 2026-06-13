using System.Net;
using BankApi.Common;
using BankApi.dal.DTOs.Transaction;
using BankApi.dal.Models;
using BankApi.dal.Models.Enums;
using BankApi.dal.Repositories;
using BankApi.Data;
using BankApi.Mappers;

namespace BankApi.Services;

public class TransactionService(
    AppDbContext db, 
    IAccountRepository accountRepository,
    ITransactionRepository transactionRepository, 
    CurrencyService currencyService, 
    TransactionMapper mapper)
{
    public async Task<TransferResponse> Transfer(int userId, TransferRequest request)
    {
        if (request.FromAccountId == request.ToAccountId)
            throw new AppException(ErrorCodes.TransferSameAccount, HttpStatusCode.BadRequest);

        var fromAccount = await accountRepository.GetByIdAsync(request.FromAccountId)
            ?? throw new AppException(ErrorCodes.AccountNotFound, HttpStatusCode.NotFound);

        var toAccount = await accountRepository.GetByIdAsync(request.ToAccountId)
            ?? throw new AppException(ErrorCodes.AccountNotFound, HttpStatusCode.NotFound);

        ValidateTransfer(fromAccount, toAccount, userId, request.Amount);

        var conversion = await currencyService.Convert(
            request.Amount,
            fromAccount.Currency,
            toAccount.Currency
        );

        using var dbTransaction = await db.Database.BeginTransactionAsync();
        try
        {
            fromAccount.Balance -= request.Amount;
            toAccount.Balance += conversion.ConvertedAmount;

            var transaction = mapper.ToEntity(request, fromAccount.Currency, conversion);
            
            db.Transactions.Add(transaction);
            await db.SaveChangesAsync();

            await db.Entry(transaction).Reference(t => t.FromAccount).LoadAsync();
            await db.Entry(transaction).Reference(t => t.ToAccount).LoadAsync();

            await dbTransaction.CommitAsync();

            return mapper.ToResponse(transaction, ErrorMessages.Get(ErrorCodes.TransferSuccess));
        }
        catch
        {
            await dbTransaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<TransferResponse>> GetByAccountId(int accountId, int userId)
    {
        var account = await accountRepository.GetByIdAsync(accountId)
            ?? throw new AppException(ErrorCodes.AccountNotFound, HttpStatusCode.NotFound);

        if (account.UserId != userId)
            throw new AppException(ErrorCodes.AccountNotBelongToUser, HttpStatusCode.Forbidden);

        var transactions = await transactionRepository.GetByAccountIdAsync(accountId);
        return transactions.Select(t => mapper.ToResponse(t, string.Empty)).ToList();
    }

    private static void ValidateTransfer(Account fromAccount, Account toAccount, int userId, decimal amount)
    {
        if (fromAccount.UserId != userId)
            throw new AppException(ErrorCodes.AccountNotBelongToUser, HttpStatusCode.Forbidden);

        if (fromAccount.Status == AccountStatus.Closed)
            throw new AppException(ErrorCodes.AccountClosed, HttpStatusCode.Conflict);

        if (toAccount.Status == AccountStatus.Closed)
            throw new AppException(ErrorCodes.AccountClosed, HttpStatusCode.Conflict);

        if (fromAccount.Balance < amount)
            throw new AppException(ErrorCodes.InsufficientFunds, HttpStatusCode.UnprocessableEntity);
    }
}