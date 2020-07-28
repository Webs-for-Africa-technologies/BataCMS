using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Models
{
    public class Checkout
    {
        private readonly AppDbContext _appDbContext;

        public Checkout(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public string CheckoutId { get; set; }

        public List<CheckoutItem> CheckoutItems { get; set; }

        public static Checkout GetCart(IServiceProvider serviceProvider)
        {
            ISession session = serviceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            var context = serviceProvider.GetService<AppDbContext>();
            string checkoutId = session.GetString("CheckoutId") ?? Guid.NewGuid().ToString();

            session.SetString("CheckoutId", checkoutId);

            return new Checkout(context) { CheckoutId = checkoutId };
        }

        public void AddItem(unitItem item, int amount)
        {
            var checkoutItem = _appDbContext.CheckoutItems.SingleOrDefault(s => s.unitItem.unitItemId == item.unitItemId && s.CheckoutId == CheckoutId);

            if (checkoutItem == null)
            {
                checkoutItem = new CheckoutItem
                {
                    CheckoutId = CheckoutId,
                    unitItem = item,
                    Amount = 1
                };

                _appDbContext.CheckoutItems.AddAsync(checkoutItem);
            }else
            {
                checkoutItem.Amount++;
            }

            _appDbContext.SaveChanges();
        }

        public int RemoveItem(unitItem item)
        {
            var checkoutItem = _appDbContext.CheckoutItems.SingleOrDefault(s => s.unitItem.unitItemId == item.unitItemId && s.CheckoutId == CheckoutId);

            var localAmount = 0; 

            if (checkoutItem != null)
            {
                if(checkoutItem.Amount > 1)
                {
                    checkoutItem.Amount--;
                    localAmount = checkoutItem.Amount;
                }else
                {
                    _appDbContext.CheckoutItems.Remove(checkoutItem);
                }
            }
            _appDbContext.SaveChanges();

            return localAmount;
        }

        public List<CheckoutItem> GetCheckoutItems()
        {
            return CheckoutItems ?? (CheckoutItems = _appDbContext.CheckoutItems.Where(c => c.CheckoutId == CheckoutId).Include(s => s.unitItem).ToList());
        }

        public void ClearCheckout()
        {
            var checkoutItems = _appDbContext.CheckoutItems.Where(c => c.CheckoutId == CheckoutId);

            _appDbContext.CheckoutItems.RemoveRange(checkoutItems);

            _appDbContext.SaveChanges();
        }

        public decimal GetCheckoutTotal()
        {
            var total = _appDbContext.CheckoutItems.Where(c => c.CheckoutId == CheckoutId).Select(c => c.unitItem.Price * c.Amount).Sum();

            return total; 
        }
    }
}
