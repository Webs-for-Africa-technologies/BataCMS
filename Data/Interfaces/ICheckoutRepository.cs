using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Interfaces
{
    public interface ICheckoutRepository
    {

        void AddItem(unitItem item, int amount);

        decimal RemoveItem(unitItem item);

        List<CheckoutItem> GetCheckoutItems();

        void ClearCheckout();

        decimal GetCheckoutTotal();
    }
}
