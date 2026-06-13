using BankApi.dal.Models;
using BankApi.dal.Models.Enums;

namespace BankApi.dal.Repositories;

public interface ICardRepository
{
    Task<bool> HasActiveCardAsync(int accountId);
    Task<List<Card>> GetByAccountIdAsync(int accountId);
    Task<Card?> GetByIdWithAccountAndUserAsync(int id);
}