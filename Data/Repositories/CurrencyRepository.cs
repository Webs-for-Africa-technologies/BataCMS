using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {

        private readonly AppDbContext _appDbContext;


        public IEnumerable<Currency> Currencies => _appDbContext.Currencies;

        public CurrencyRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public Currency AddCurrency(Currency currency)
        {
            _appDbContext.Currencies.AddAsync(currency);
            _appDbContext.SaveChanges();
            return currency;
        }

        public Currency GetCurrentCurrency()
        {
            return _appDbContext.Currencies.SingleOrDefault(p => p.isCurrent == true);
        }

        public void SetCurrentCurrency(Currency currency)
        {
            var CurrentCurrency = _appDbContext.Currencies.Where(p => p.isCurrent == true);

            foreach (var item in CurrentCurrency)
            {
                item.isCurrent = false;
            }

            _appDbContext.SaveChanges();

            Currency newCurrent = _appDbContext.Currencies.SingleOrDefault(p => p.CurrencyId == currency.CurrencyId);

            newCurrent.isCurrent = true;

            _appDbContext.Currencies.Update(newCurrent);

            _appDbContext.SaveChanges(); 

        }

        public Currency UpdateCurrency(Currency updatedCurrency)
        {
            _appDbContext.SaveChanges();
            return updatedCurrency;
        }

        public Currency GetCurrencyById(int CurrencyId)
        {
            return _appDbContext.Currencies.FirstOrDefault(p => p.CurrencyId == CurrencyId);
        }

        public Currency GetCurrencyByName(string CurrencyName)
        {
            return _appDbContext.Currencies.FirstOrDefault(p => p.CurrencyName == CurrencyName);
        }

        public void  DeleteCurrency(Currency currency)
        {
            _appDbContext.Currencies.Remove(currency);
            _appDbContext.SaveChanges();
        }
    }
}
