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

            var checkoutItems = _checkkout.CheckoutItems;

            foreach (var item in checkoutItems)
            {
                var purchasedItem = new PurchasedItem {
                    Amount = item.Amount,
                    unitItemId = item.unitItem.unitItemId,
                    PurchaseId = purchase.PurchaseID,
                    Price = item.unitItem.Price,
                
                };
                _appDbContext.PurchasedItems.Add(purchasedItem);
            }

            _appDbContext.SaveChanges();
        }
    }
}
