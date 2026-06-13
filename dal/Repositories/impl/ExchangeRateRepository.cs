using BankApi.dal.Models;
using BankApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BankApi.dal.Repositories.impl;

public class ExchangeRateRepository(AppDbContext db) : IExchangeRateRepository
{
    public async Task<List<ExchangeRate>> GetAllAsync() =>
        await db.ExchangeRates.AsNoTracking().ToListAsync();
}