using BankApi.dal.Models;

namespace BankApi.dal.Repositories;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(int id);
    Task<Account?> GetByIdWithCardsAsync(int id);
    Task<List<Account>> GetByUserIdAsync(int userId);
    Task<List<Account>> GetActiveByUserIdExceptAsync(int userId, int excludeAccountId);
}