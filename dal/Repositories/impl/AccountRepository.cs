using BankApi.dal.Models;
using BankApi.dal.Models.Enums;
using BankApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BankApi.dal.Repositories.impl;

public class AccountRepository(AppDbContext db) : IAccountRepository
{
    public async Task<Account?> GetByIdAsync(int id)
    {
        return await db.Accounts
            .FirstOrDefaultAsync(a => a.Id == id);
    }
    
    public async Task<Account?> GetByIdWithCardsAsync(int id)
    {
        return await db.Accounts
            .Include(a => a.Cards)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Account>> GetByUserIdAsync(int userId)
    {
        return await db.Accounts
            .Where(a => a.UserId == userId)
            .ToListAsync();
    }

    public async Task<List<Account>> GetActiveByUserIdExceptAsync(int userId, int excludeAccountId)
    {
        return await db.Accounts
            .Where(a => a.UserId == userId && a.Id != excludeAccountId && a.Status == AccountStatus.Active)
            .ToListAsync();
    }
}