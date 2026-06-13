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
}