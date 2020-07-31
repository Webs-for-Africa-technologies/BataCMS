using BataCMS.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Components
{
    public class CurrencySelectMenu : ViewComponent
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencySelectMenu(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public IViewComponentResult Invoke()
        {
            var currencies = _currencyRepository.Currencies.OrderBy(p => p.CurrencyName);
            return View(currencies);
        }

    }
}
