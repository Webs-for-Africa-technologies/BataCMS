using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Interfaces
{
    public interface IPurchaseRepository
    {
        void CreatePurchase(Purchase purchase);

        IEnumerable<Purchase> Purchases { get; }

        Purchase GetPurchaseById(int purchaseId);

    }
}
