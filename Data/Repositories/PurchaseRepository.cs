using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
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
        public PurchaseRepository(AppDbContext appDbContext, ICheckoutRepository checkoutRepository, ICurrencyRepository currencyRepository)
        {
            _appDbContext = appDbContext;
            _checkoutRepository = checkoutRepository;
            _currencyRepository = currencyRepository;
        }

        public IEnumerable<Purchase> Purchases => _appDbContext.Purchases.Include(p => p.PaymentMethods).OrderByDescending(p => p.PurchaseDate);

        public async Task CreatePurchaseAsync(Purchase purchase)
        {
            purchase.PurchaseDate = DateTime.Now;
            await _appDbContext.AddAsync(purchase);
                
            //Add a purchase to Db to make reference to the FK. 
            await _appDbContext.SaveChangesAsync();

            var checkoutItems = await _checkoutRepository.GetCheckoutItemsAsync();

            decimal purchaseTotal = 0M;

            foreach (var item in checkoutItems)
            {
                var purchasedItem = new PurchasedItem {
                    Amount = item.Amount,
                    unitItemId = item.unitItem.unitItemId,
                    PurchaseId = purchase.PurchaseId,
                    Price = item.unitItem.Price,
                };
                purchaseTotal += (item.unitItem.Price*item.Amount) * _currencyRepository.GetCurrentCurrency().Rate;
                await _appDbContext.PurchasedItems.AddAsync(purchasedItem);
            }
            purchase.PurchasesTotal = purchaseTotal;


            PaymentMethod paymentMethod = new PaymentMethod { AmountPaid = purchaseTotal, PaymentMethodName = _currencyRepository.GetCurrentCurrency().CurrencyName };
            
            //initialize the list
            purchase.PaymentMethods = new List<PaymentMethod>();

            purchase.PaymentMethods.Add(paymentMethod);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Purchase> GetPurchaseByIdAsync(int purchaseId)
        {
            return await _appDbContext.Purchases.Include(p => p.PaymentMethods).FirstOrDefaultAsync(p => p.PurchaseId == purchaseId);
        }
    }
}
