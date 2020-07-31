using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Repositories
{
    public class CheckoutRepository : ICheckoutRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly Checkout _checkout;
        private readonly ICurrencyRepository _currencyRepository;

        public CheckoutRepository(AppDbContext appDbContext, Checkout checkout, ICurrencyRepository currencyRepository)
        {
            _appDbContext = appDbContext;
            _checkout = checkout;
            _currencyRepository = currencyRepository;

        }

        public void AddItem(unitItem item, int amount)
        {
            var checkoutItem = _appDbContext.CheckoutItems.SingleOrDefault(s => s.unitItem.unitItemId == item.unitItemId && s.CheckoutId == _checkout.CheckoutId);

            if (checkoutItem == null)
            {
                checkoutItem = new CheckoutItem
                {
                    CheckoutId = _checkout.CheckoutId,
                    unitItem = item,
                    Amount = 1
                };

                _appDbContext.CheckoutItems.AddAsync(checkoutItem);
            }
            else
            {
                checkoutItem.Amount++;
            }

            _appDbContext.SaveChanges();
        }

        public void ClearCheckout()
        {
            var checkoutItems = _appDbContext.CheckoutItems.Where(c => c.CheckoutId == _checkout.CheckoutId);

            _appDbContext.CheckoutItems.RemoveRange(checkoutItems);

            _appDbContext.SaveChanges();
        }

        public Checkout GetCart(IServiceProvider serviceProvider)
        {
            ISession session = serviceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            var context = serviceProvider.GetService<AppDbContext>();
            string checkoutId = session.GetString("CheckoutId") ?? Guid.NewGuid().ToString();

            session.SetString("CheckoutId", checkoutId);

            return new Checkout{ CheckoutId = checkoutId };
        }

        public List<CheckoutItem> GetCheckoutItems()
        {
            return _checkout.CheckoutItems ?? (_checkout.CheckoutItems = _appDbContext.CheckoutItems.Where(c => c.CheckoutId == _checkout.CheckoutId).Include(s => s.unitItem).ToList());

        }

        public decimal GetCheckoutTotal()
        {
            var total = _appDbContext.CheckoutItems.Where(c => c.CheckoutId == _checkout.CheckoutId).Select(c => c.unitItem.Price * c.Amount).Sum();

            return total * _currencyRepository.GetCurrentCurrency().Rate;
        }

        public decimal RemoveItem(unitItem item)
        {
            var checkoutItem = _appDbContext.CheckoutItems.SingleOrDefault(s => s.unitItem.unitItemId == item.unitItemId && s.CheckoutId == _checkout.CheckoutId);

            var localAmount = 0;

            if (checkoutItem != null)
            {
                if (checkoutItem.Amount > 1)
                {
                    checkoutItem.Amount--;
                    localAmount = checkoutItem.Amount;
                }
                else
                {
                    _appDbContext.CheckoutItems.Remove(checkoutItem);
                }
            }
            _appDbContext.SaveChanges();

            return localAmount;
        }
    }
}
