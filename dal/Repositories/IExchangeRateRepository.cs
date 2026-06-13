using BankApi.dal.Models;

namespace BankApi.dal.Repositories;

public interface IExchangeRateRepository
{
    Task<List<ExchangeRate>> GetAllAsync();
}