using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Interfaces
{
    public interface IInvoiceRepository
    {
        Invoice GetInvoice(int id);

        Task AddInvoice(Invoice invoice);

    }
}
