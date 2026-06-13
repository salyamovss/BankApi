using BankApi.dal.DTOs.User;
using BankApi.dal.Models;

namespace BankApi.dal.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdWithDetailsAsync(int id);
    
    Task<User?> GetByEmailWithDetailsAsync(string email);
    
    Task<List<User>> GetPagedListAsync(int page, int pageSize);
    Task<List<User>> GetFilteredAsync(UserFilterRequest filter);
    Task AddAsync(User user);
    Task SaveChangesAsync();
    Task<User?> GetByIdWithAccountsAsync(int id);
}