using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Repositories
{
    public class PaymentMethodRepository :IPaymentMethodRepository
    {
        private readonly AppDbContext _appDbContext;
        public PaymentMethodRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<PaymentMethod> PaymentMethods => _appDbContext.PaymentMethods;
    }
}
