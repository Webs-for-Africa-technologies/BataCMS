using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Interfaces
{
    public interface IInvoiceRepository
    {
        IEnumerable<Invoice> Invoices { get; }

        Invoice GetInvoice(int id);

        Task AddInvoice(Invoice invoice);

        IEnumerable<Invoice> GetUserInvoices(string userId);

    }
}
