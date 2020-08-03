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

        void SetCurrentCurrency(Currency currency);

        Currency AddCurrency(Currency currency);

        Currency UpdateCurrency(Currency updatedCurrency);

        Currency GetCurrencyById(int CurrencyId);

        Currency GetCurrencyByName(string CurrencyName);

        void DeleteCurrency(Currency currency);
    }
}
