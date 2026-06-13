using BankApi.dal.Models;

namespace BankApi.dal.Repositories;

public interface ITransactionRepository
{
    Task<List<Transaction>> GetByAccountIdAsync(int accountId);
}