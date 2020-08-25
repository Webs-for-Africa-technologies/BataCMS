using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public void CreatePurchase(Purchase purchase)
        {
            purchase.PurchaseDate = DateTime.Now;
            _appDbContext.AddAsync(purchase);

            //Add a purchase to Db to make reference to the FK. 
            _appDbContext.SaveChanges();

            var checkoutItems = _checkoutRepository.GetCheckoutItems();

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
                _appDbContext.PurchasedItems.AddAsync(purchasedItem);
            }
            purchase.PurchasesTotal = purchaseTotal;


            PaymentMethod paymentMethod = new PaymentMethod { AmountPaid = purchaseTotal, PaymentMethodName = _currencyRepository.GetCurrentCurrency().CurrencyName };
            
            //initialize the list
            purchase.PaymentMethods = new List<PaymentMethod>();

            purchase.PaymentMethods.Add(paymentMethod);
            _appDbContext.SaveChanges();
        }

        public Purchase GetPurchaseById(int purchaseId)
        {
            return _appDbContext.Purchases.Include(p => p.PaymentMethods).FirstOrDefault(p => p.PurchaseId == purchaseId);
        }
    }
}
