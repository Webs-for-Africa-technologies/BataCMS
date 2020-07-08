using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Repositories
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly Checkout _checkout;

        public PaymentMethodRepository(AppDbContext appDbContext, Checkout checkout )
        {
            _appDbContext = appDbContext;
            _checkout = checkout;
        }

        public void CreatePaymentMethod(PaymentMethod paymentMethod)
        {

            var checkoutItems = _checkout.CheckoutItems;



            decimal purchaseTotal = 0M;

            foreach (var item in checkoutItems)
            {
                purchaseTotal += item.unitItem.Price;
            }

            // if the amount paid is less than the total purchase, add another payment method comment#0002
            if (purchaseTotal <= paymentMethod.AmountPaid)
            {
                //addPurchase Payment method
                _appDbContext.Add(paymentMethod);
            }

            _appDbContext.SaveChanges();



            _appDbContext.SaveChanges();
        }
    }
}
