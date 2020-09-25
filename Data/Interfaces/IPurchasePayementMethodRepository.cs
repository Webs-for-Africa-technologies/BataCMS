using BataCMS.Data.Models;
using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Interfaces
{
    public interface IPurchasePayementMethodRepository
    {
        Lease GetPurchasePaymentMethodByPurchaseId(int purchaseId);

        void AddPurchasePaymentMethod(Lease purchasePaymentMethod);
    }
}
