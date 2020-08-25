using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Interfaces
{
    public interface ICurrencyRepository
    {
        IEnumerable<Currency> Currencies { get; }
         
        Currency GetCurrentCurrency();

        Task SetCurrentCurrencyAsync(Currency currencyId);

        Task<Currency> AddCurrencyAsync(Currency currency);

        Task UpdateCurrencyAsync(Currency updatedCurrency);

        Task<Currency> GetCurrencyByIdAsync(int CurrencyId);

        Task<Currency> GetCurrencyByNameAsync(string CurrencyName);

        Task DeleteCurrencyAsync(Currency currency);
    }
}
