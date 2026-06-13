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
}