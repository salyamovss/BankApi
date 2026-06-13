using BankApi.dal.Models;
using BankApi.Data;
using BankApi.dal.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BankApi.dal.Repositories.impl;

public class CardRepository(AppDbContext db) : ICardRepository
{
    public async Task<bool> HasActiveCardAsync(int accountId)
    {
        return await db.Cards.AnyAsync(c => c.AccountId == accountId && c.Status == CardStatus.Active);
    }
    
    public async Task<List<Card>> GetByAccountIdAsync(int accountId)
    {
        return await db.Cards
            .Where(c => c.AccountId == accountId)
            .ToListAsync();
    }
    
    public async Task<Card?> GetByIdWithAccountAndUserAsync(int id)
    {
        return await db.Cards
            .Include(c => c.Account)
            .ThenInclude(a => a.User)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task<Card?> GetByIdWithAccountAsync(int id) =>
        await db.Cards
            .Include(c => c.Account)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Account?> GetAccountWithUserAsync(int accountId) =>
        await db.Accounts
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == accountId);

    public async Task<bool> AccountExistsAsync(int accountId) =>
        await db.Accounts.AnyAsync(a => a.Id == accountId);

    public async Task AddAsync(Card card) =>
        await db.Cards.AddAsync(card);

    public async Task SaveChangesAsync() =>
        await db.SaveChangesAsync();
    
    
}