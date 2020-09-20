using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Repositories
{
    public class PurchasedItemRepository : IPurchasedItemRepository
    {
        private readonly AppDbContext _appDbContext;

        public PurchasedItemRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<PurchasedItem> GetPurchasedItemsById(int purchaseId)
        {
            return _appDbContext.PurchasedItems.Where(p => p.TransactionId == purchaseId);
        }
    }
}
