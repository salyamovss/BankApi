using BankApi.dal.Models;

namespace BankApi.dal.Repositories;

public interface IPhoneRepository
{
    Task<Phone?> GetByIdAsync(int id);
    Task<bool> ExistsAsync(int userId, string number);
    Task<bool> UserExistsAsync(int userId);
    Task<bool> IsNumberTakenAsync(string number);
    Task<int> CountByUserIdAsync(int userId);
    Task AddAsync(Phone phone);
    Task RemoveAsync(Phone phone);
    Task SaveChangesAsync();
}