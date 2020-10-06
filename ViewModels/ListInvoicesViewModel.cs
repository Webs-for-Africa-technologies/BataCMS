using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.ViewModels
{
    public class ListInvoicesViewModel
    {
        public IEnumerable<Invoice> Invoices { get; set; }

    }
}
