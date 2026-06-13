using BankApi.dal.Models;
using BankApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BankApi.dal.Repositories.impl;

public class TransactionRepository(AppDbContext db) : ITransactionRepository
{
    public async Task<List<Transaction>> GetByAccountIdAsync(int accountId)
    {
        return await db.Transactions
            .Include(t => t.FromAccount)
            .Include(t => t.ToAccount)
            .Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }
    
    /// <summary>
    /// Выполняет перевод между счетами в рамках одной транзакции БД.
    /// При ошибке автоматически откатывает изменения.
    /// </summary>
    public async Task<Transaction> ExecuteTransferAsync(Account fromAccount, Account toAccount, Transaction transaction)
    {
        using var dbTransaction = await db.Database.BeginTransactionAsync();
        try
        {
            db.Accounts.Update(fromAccount);
            db.Accounts.Update(toAccount);

            db.Transactions.Add(transaction);
            await db.SaveChangesAsync();

            await db.Entry(transaction).Reference(t => t.FromAccount).LoadAsync();
            await db.Entry(transaction).Reference(t => t.ToAccount).LoadAsync();

            await dbTransaction.CommitAsync();
            return transaction;
        }
        catch
        {
            await dbTransaction.RollbackAsync();
            throw;
        }
    }
}