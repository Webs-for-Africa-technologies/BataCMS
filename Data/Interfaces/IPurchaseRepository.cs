using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Interfaces
{
    public interface IPurchaseRepository
    {
        Task CreatePurchaseAsync(Purchase purchase);

        IEnumerable<Purchase> Purchases { get; }

        Task<Purchase> GetPurchaseByIdAsync(int purchaseId);

    }
}
