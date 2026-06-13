using BankApi.dal.DTOs.User;
using BankApi.dal.Models;
using BankApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BankApi.dal.Repositories.impl;

public class UserRepository(AppDbContext db) : IUserRepository
{
    public async Task<User?> GetByIdWithDetailsAsync(int id)
    {
        return await db.Users
            .Include(u => u.Passport)
            .Include(u => u.Phones)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailWithDetailsAsync(string email)
    {
        return await db.Users
            .Include(u => u.Passport)
            .Include(u => u.Phones)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<List<User>> GetPagedListAsync(int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 20;

        int skip = (page - 1) * pageSize;

        return await db.Users
            .AsNoTracking()
            .Include(u => u.Passport)
            .Include(u => u.Phones)
            .OrderByDescending(u => u.CreatedAt)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();
    }
    
    public async Task<List<User>> GetFilteredAsync(UserFilterRequest filter)
    {
        var query = db.Users
            .Include(u => u.Passport)
            .Include(u => u.Phones)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.FirstName))
            query = query.Where(u => u.FirstName.ToLower().Contains(filter.FirstName.ToLower()));

        if (!string.IsNullOrWhiteSpace(filter.LastName))
            query = query.Where(u => u.LastName.ToLower().Contains(filter.LastName.ToLower()));

        if (!string.IsNullOrWhiteSpace(filter.Email))
            query = query.Where(u => u.Email.Contains(filter.Email.ToLower()));

        if (filter.IsActive.HasValue)
            query = query.Where(u => u.IsActive == filter.IsActive.Value);

        int page = filter.Page < 1 ? 1 : filter.Page;
        int pageSize = filter.PageSize < 1 || filter.PageSize > 100 ? 20 : filter.PageSize;

        return await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    
}