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

        public async Task AddItemAsync(RentalAsset item, int amount, string selectedOptions)
        {
            var checkoutItem = await _appDbContext.CheckoutItems.SingleOrDefaultAsync(s => s.RentalAsset.RentalAssetId == item.RentalAssetId && s.CheckoutId == _checkout.CheckoutId);

            if (checkoutItem == null)
            {
                checkoutItem = new CheckoutItem
                {
                    CheckoutId = _checkout.CheckoutId,
                    RentalAsset = item,
                    Amount = 1,
                    selectedOptions = selectedOptions
                };

                await _appDbContext.CheckoutItems.AddAsync(checkoutItem);
            }
            else
            {
                checkoutItem.Amount++;
            }

            await _appDbContext.SaveChangesAsync();
        }

        public async Task ClearCheckoutAsync()
        {
            var checkoutItems = _appDbContext.CheckoutItems.Where(c => c.CheckoutId == _checkout.CheckoutId);

            _appDbContext.CheckoutItems.RemoveRange(checkoutItems);

            await _appDbContext.SaveChangesAsync();
        }


        public async Task<List<CheckoutItem>> GetCheckoutItemsAsync()
        {
            return _checkout.CheckoutItems ?? (_checkout.CheckoutItems = await _appDbContext.CheckoutItems.Where(c => c.CheckoutId == _checkout.CheckoutId).Include(s => s.RentalAsset).ToListAsync());

        }

        public decimal GetCheckoutTotal()
        {
            var total = _appDbContext.CheckoutItems.Where(c => c.CheckoutId == _checkout.CheckoutId).Select(c => c.RentalAsset.Price * c.Amount).Sum();

            return total * _currencyRepository.GetCurrentCurrency().Rate;
        }

        public async Task<decimal> RemoveItemAsync(RentalAsset item)
        {
            var checkoutItem = await _appDbContext.CheckoutItems.SingleOrDefaultAsync(s => s.RentalAsset.RentalAssetId == item.RentalAssetId && s.CheckoutId == _checkout.CheckoutId);

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
            await _appDbContext.SaveChangesAsync();

            return localAmount;
        }
    }
}
