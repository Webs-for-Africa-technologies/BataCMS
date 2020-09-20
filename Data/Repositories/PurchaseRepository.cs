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
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IHubContext<SignalServer> _hubContext;

        public PurchaseRepository(AppDbContext appDbContext, ICheckoutRepository checkoutRepository, ICurrencyRepository currencyRepository, IHubContext<SignalServer> hubContext)
        {
            _appDbContext = appDbContext;
            _checkoutRepository = checkoutRepository;
            _currencyRepository = currencyRepository;
            _hubContext = hubContext;
        }

        public IEnumerable<Transaction> Purchases => _appDbContext.Transactions.Include(p => p.Rental).OrderByDescending(p => p.TransactionDate);

        public async Task CreatePurchaseAsync(Transaction purchase)
        {
            purchase.TransactionDate = DateTime.Now;
            await _appDbContext.AddAsync(purchase);
                
            //Add a purchase to Db to make reference to the FK. 
            await _appDbContext.SaveChangesAsync();

            var checkoutItems = await _checkoutRepository.GetCheckoutItemsAsync();

            decimal purchaseTotal = 0M;

            foreach (var item in checkoutItems)
            {
                var purchasedItem = new PurchasedItem {
                    Amount = item.Amount,
                    unitItemId = item.RentalAsset.RentalAssetId,
                    TransactionId = purchase.TransactionId,
                    Price = item.RentalAsset.Price,
                    selectedOptionData = item.selectedOptions,
                };
                purchaseTotal += (item.RentalAsset.Price*item.Amount) * _currencyRepository.GetCurrentCurrency().Rate;
                await _appDbContext.PurchasedItems.AddAsync(purchasedItem);
            }
            purchase.TransactionTotal = purchaseTotal;


            PaymentMethod paymentMethod = new PaymentMethod { AmountPaid = purchaseTotal, PaymentMethodName = _currencyRepository.GetCurrentCurrency().CurrencyName };
            
            //initialize the list
           // purchase.PaymentMethods = new List<PaymentMethod>();

            ///purchase.PaymentMethods.Add(paymentMethod);
            await _appDbContext.SaveChangesAsync();

            var OrderCount = (Purchases.Where(p => p.isDelivered == false)).Count();
           await _hubContext.Clients.All.SendAsync("updatePurchase", OrderCount);
        }

        public async Task DeletePurchasesAsync()
        {
            _appDbContext.PaymentMethods.RemoveRange(_appDbContext.PaymentMethods.Where(u => u.isConfirmed == true));
            _appDbContext.Transactions.RemoveRange(_appDbContext.Transactions.Where(u => u.isDelivered == true));
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Transaction> GetPurchaseByIdAsync(int purchaseId)
        {
            return await _appDbContext.Transactions.Include(p => p.Rental).FirstOrDefaultAsync(p => p.TransactionId == purchaseId);
        }

        public async Task UpdatePurchaseAsync(Transaction purchase)
        {
            _appDbContext.Transactions.Update(purchase);
            await _appDbContext.SaveChangesAsync();

            var OrderCount = (Purchases.Where(p => p.isDelivered == false)).Count();
            await _hubContext.Clients.All.SendAsync("updatePurchase", OrderCount);
        }
    }
}
