using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Models
{
    public class CurrentCurrency
    {
        public int CurrencyId { get; set; }

        public static CurrentCurrency GetCurrentCurrency(IServiceProvider serviceProvider)
        {
            ISession session = serviceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            var context = serviceProvider.GetService<AppDbContext>();
            int? currencyId = session.GetInt32("CurrencyId");

            if (currencyId != null)
            {
                return new CurrentCurrency { CurrencyId = (int)currencyId };
            }

            return new CurrentCurrency { CurrencyId = -1 }; 
        }
    }
}
