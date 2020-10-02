using BataCMS.Data.Models;
using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Interfaces
{
    public interface IRentalAssetRepository
    {
        IEnumerable<RentalAsset> rentalAssets { get; }

        Task<RentalAsset> GetItemByIdAsync(int itemId);

        Task<int> DeleteItem(int itemId);

        Task<RentalAsset> AddAsync(RentalAsset item);

        Task EditItemAsync(RentalAsset updatedItem);

        public Task BookAsset(DateTime bookedTill, int assetID);

        public Task EndBooking(int assetId);
    }
}
