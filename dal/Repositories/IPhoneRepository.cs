using BankApi.dal.Models;

namespace BankApi.dal.Repositories;

public interface IPhoneRepository
{
    Task<Phone?> GetByIdAsync(int id);
    Task<bool> ExistsAsync(int userId, string number);
}