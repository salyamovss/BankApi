using BankApi.dal.Models;

namespace BankApi.dal.Repositories;

public interface ITransactionRepository
{
    Task<List<Transaction>> GetByAccountIdAsync(int accountId);
    Task<Transaction> ExecuteTransferAsync(Account fromAccount, Account toAccount, Transaction transaction);
}