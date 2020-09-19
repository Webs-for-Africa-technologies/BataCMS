using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Interfaces
{
    public interface IUnitItemRepository
    {
        IEnumerable<RentalAsset> unitItems { get; }

        Task<RentalAsset> GetItemByIdAsync(int itemId);

        /*Task<int> DeleteItem(int itemId);*/

        Task<RentalAsset> AddAsync (RentalAsset item);

        Task EditItemAsync(RentalAsset updatedItem);
    }
}
