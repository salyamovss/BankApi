using BankApi.dal.Models;
using BankApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BankApi.dal.Repositories.impl;

public class PhoneRepository(AppDbContext db) : IPhoneRepository
{
    public async Task<Phone?> GetByIdAsync(int id)
    {
        return await db.Phones
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> ExistsAsync(int userId, string number)
    {
        return await db.Phones.AnyAsync(p => p.UserId == userId && p.Number == number);
    }
    
    public async Task<bool> UserExistsAsync(int userId) =>
        await db.Users.AnyAsync(u => u.Id == userId && u.IsActive);

    public async Task<bool> IsNumberTakenAsync(string number) =>
        await db.Phones.AnyAsync(p => p.Number == number);

    public async Task<int> CountByUserIdAsync(int userId) =>
        await db.Phones.CountAsync(p => p.UserId == userId);

    public async Task AddAsync(Phone phone) =>
        await db.Phones.AddAsync(phone);

    public async Task RemoveAsync(Phone phone) =>
        db.Phones.Remove(phone);

    public async Task SaveChangesAsync() =>
        await db.SaveChangesAsync();
    
}