using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Repositories
{
    public class PurchasePaymentMethodRepository : IPurchasePayementMethodRepository
    {
        private readonly AppDbContext _appDbContext;

        public PurchasePaymentMethodRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void AddPurchasePaymentMethod(PurchasePaymentMethod purchasePaymentMethod)
        {
            _appDbContext.AddAsync(purchasePaymentMethod);
            _appDbContext.SaveChanges();
        }

        public PurchasePaymentMethod GetPurchasePaymentMethodByPurchaseId(int purchaseId)
        {
            return _appDbContext.PurchasePaymentMethod.FirstOrDefault(p => p.PurchaseId == purchaseId);
        }
    }
}
