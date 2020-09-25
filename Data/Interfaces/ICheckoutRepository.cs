using BataCMS.Data.Models;
using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Interfaces
{
    public interface ICheckoutRepository
    {

        Task AddItemAsync(RentalAsset rentalAsset, int amount, string selectedOptions);

        Task<decimal> RemoveItemAsync(RentalAsset rentalAsset);

        Task<List<CheckoutItem>> GetCheckoutItemsAsync();

         Task ClearCheckoutAsync();

        decimal GetCheckoutTotal();
    }
}
