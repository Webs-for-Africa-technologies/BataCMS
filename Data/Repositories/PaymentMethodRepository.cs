using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Repositories
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly ICheckoutRepository _checkoutRepository;

        public PaymentMethodRepository(AppDbContext appDbContext, ICheckoutRepository checkoutRepository )
        {
            _appDbContext = appDbContext;
            _checkoutRepository = checkoutRepository;
        }

        public void CreatePaymentMethod(PaymentMethod paymentMethod)
        {

            var checkoutItems = _checkoutRepository.GetCheckoutItems();



            decimal purchaseTotal = 0M;

            foreach (var item in checkoutItems)
            {
                purchaseTotal += item.unitItem.Price;
            }

            // if the amount paid is less than the total purchase, add another payment method comment#0002
            if (purchaseTotal <= paymentMethod.AmountPaid)
            {
                //addPurchase Payment method
                _appDbContext.AddAsync(paymentMethod);
            }
            _appDbContext.SaveChanges();
        }

        public PaymentMethod GetPaymentMethodById(int paymentMethodId)
        {
           return _appDbContext.PaymentMethods.FirstOrDefault(p => p.PaymentMethodId == paymentMethodId);
        }
    }
}
