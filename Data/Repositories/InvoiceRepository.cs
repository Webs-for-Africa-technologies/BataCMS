using BataCMS.Data;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly AppDbContext _appDbContext;

        public InvoiceRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task AddInvoice(Invoice invoice)
        {
            await _appDbContext.AddAsync(invoice);
            await _appDbContext.SaveChangesAsync();
        }

        public Invoice GetInvoice(int id)
        {
            return _appDbContext.Invoices.FirstOrDefault(p => p.InvoiceId == id);
        }
    }
}
