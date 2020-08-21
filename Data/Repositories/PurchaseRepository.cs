using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BataCMS.Data.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly ICheckoutRepository _checkoutRepository;
        public PurchaseRepository(AppDbContext appDbContext, ICheckoutRepository checkoutRepository)
        {
            _appDbContext = appDbContext;
            _checkoutRepository = checkoutRepository;
        }

        public IEnumerable<Purchase> Purchases => _appDbContext.Purchases.OrderByDescending(p => p.PurchaseDate);

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
                purchaseTotal += (item.unitItem.Price*item.Amount);
                _appDbContext.PurchasedItems.AddAsync(purchasedItem);
            }
            purchase.PurchasesTotal = purchaseTotal;

            _appDbContext.SaveChanges();
        }

        public Purchase GetPurchaseById(int purchaseId)
        {
            return _appDbContext.Purchases.FirstOrDefault(p => p.PurchaseId == purchaseId);
        }
    }
}
