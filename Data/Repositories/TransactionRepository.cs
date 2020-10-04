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


        public TransactionRepository(AppDbContext appDbContext, IHubContext<SignalServer> hubContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Transaction> Transactions => _appDbContext.Transactions.Include(p => p.Lease).OrderByDescending(p => p.TransactionDate);

        public async Task<int> CreateTransactionAsync(Transaction transaction)
        {
            await _appDbContext.AddAsync(transaction);
            //Add a purchase to Db to make reference to the FK. 
            return await _appDbContext.SaveChangesAsync();
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
