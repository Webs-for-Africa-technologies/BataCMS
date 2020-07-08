using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using System;


namespace BataCMS.Data.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly Checkout _checkkout;
        public PurchaseRepository(AppDbContext appDbContext, Checkout checkout)
        {
            _appDbContext = appDbContext;
            _checkkout = checkout;
        }

        public void CreatePurchase(Purchase purchase)
        {
            purchase.PurchaseDate = DateTime.Now;
            _appDbContext.Add(purchase);

            //Add a purchase to Db to make reference to the FK. 
            _appDbContext.SaveChanges();

            var checkoutItems = _checkkout.CheckoutItems;

            decimal purchaseTotal = 0M;

            foreach (var item in checkoutItems)
            {
                var purchasedItem = new PurchasedItem {
                    Amount = item.Amount,
                    unitItemId = item.unitItem.unitItemId,
                    PurchaseId = purchase.PurchaseId,
                    Price = item.unitItem.Price,
                };
                purchaseTotal += item.unitItem.Price;
                _appDbContext.PurchasedItems.Add(purchasedItem);
            }
            purchase.PurchasesTotal = purchaseTotal;

            _appDbContext.SaveChanges();
        }
    }
}
