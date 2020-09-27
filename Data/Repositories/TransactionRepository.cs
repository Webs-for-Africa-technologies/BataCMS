using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using BataCMS.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly ICurrencyRepository _currencyRepository;


        public TransactionRepository(AppDbContext appDbContext, ICheckoutRepository checkoutRepository, ICurrencyRepository currencyRepository, IHubContext<SignalServer> hubContext)
        {
            _appDbContext = appDbContext;
            _checkoutRepository = checkoutRepository;
            _currencyRepository = currencyRepository;
        }

        public IEnumerable<Transaction> Purchases => _appDbContext.Transactions.Include(p => p.Lease).OrderByDescending(p => p.TransactionDate);

        public async Task CreateTransactionAsync(Transaction transaction)
        {
            transaction.TransactionDate = DateTime.Now;
            await _appDbContext.AddAsync(transaction);
            //Add a purchase to Db to make reference to the FK. 
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeletePurchasesAsync()
        {
            _appDbContext.PaymentMethods.RemoveRange(_appDbContext.PaymentMethods.Where(u => u.isConfirmed == true));
            _appDbContext.Transactions.RemoveRange(_appDbContext.Transactions.Where(u => u.isDelivered == true));
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Transaction> GetPurchaseByIdAsync(int purchaseId)
        {
            return await _appDbContext.Transactions.Include(p => p.Lease).FirstOrDefaultAsync(p => p.TransactionId == purchaseId);
        }

        public Task UpdatePurchaseAsync(Transaction purchase)
        {
            throw new NotImplementedException();
        }
    }
}
