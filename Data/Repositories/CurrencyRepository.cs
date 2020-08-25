using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {

        private readonly AppDbContext _appDbContext;
        private readonly CurrentCurrency _currentCurrency;

        public IEnumerable<Currency> Currencies => _appDbContext.Currencies;

        public CurrencyRepository(AppDbContext appDbContext, CurrentCurrency currentCurrency)
        {
            _appDbContext = appDbContext;
            _currentCurrency = currentCurrency; 
        }
        public async Task<Currency> AddCurrencyAsync(Currency currency)
        {
            await _appDbContext.Currencies.AddAsync(currency);
            await _appDbContext.SaveChangesAsync();
            return currency;
        }

        public Currency GetCurrentCurrency()
        {
            int sessionCurrentCurrency = _currentCurrency.CurrencyId;

            if (sessionCurrentCurrency == -1)
            {
                return _appDbContext.Currencies.SingleOrDefault(p => p.isCurrent == true);
            }

            return _appDbContext.Currencies.SingleOrDefault(p => p.CurrencyId == sessionCurrentCurrency);
        }

        public async Task SetCurrentCurrencyAsync(Currency currency)
        {
            var CurrentCurrency = _appDbContext.Currencies.Where(p => p.isCurrent == true);

            foreach (var item in CurrentCurrency)
            {
                item.isCurrent = false;
            }

            await _appDbContext.SaveChangesAsync();

            Currency newCurrent = await _appDbContext.Currencies.SingleOrDefaultAsync(p => p.CurrencyId == currency.CurrencyId);

            newCurrent.isCurrent = true;

            _appDbContext.Currencies.Update(newCurrent);

            await _appDbContext.SaveChangesAsync(); 

        }

        public async Task UpdateCurrencyAsync(Currency updatedCurrency)
        {
             _appDbContext.Currencies.Update(updatedCurrency);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Currency> GetCurrencyByIdAsync(int CurrencyId)
        {
            return await _appDbContext.Currencies.FirstOrDefaultAsync(p => p.CurrencyId == CurrencyId);
        }

        public async Task<Currency> GetCurrencyByNameAsync(string CurrencyName)
        {
            return await _appDbContext.Currencies.FirstOrDefaultAsync(p => p.CurrencyName == CurrencyName);
        }

        public async Task DeleteCurrencyAsync(Currency currency)
        {
            _appDbContext.Currencies.Remove(currency);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
