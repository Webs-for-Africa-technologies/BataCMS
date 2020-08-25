using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Interfaces
{
    public interface ICheckoutRepository
    {

        Task AddItemAsync(unitItem item, int amount);

        Task<decimal> RemoveItemAsync(unitItem item);

        Task<List<CheckoutItem>> GetCheckoutItemsAsync();

         Task ClearCheckoutAsync();

        decimal GetCheckoutTotal();
    }
}
