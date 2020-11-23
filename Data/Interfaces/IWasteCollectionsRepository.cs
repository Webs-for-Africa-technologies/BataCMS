using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Interfaces
{
    public interface IWasteCollectionsRepository
    {
        IEnumerable<WasteCollection> WasteCollections { get; }

        public Task<WasteCollection> AddWasteCollectionAsync(WasteCollection wasteCollection);

        Task<WasteCollection> GetWasteCollectionAsync(int id);

        Task UpdateWasteCollection(WasteCollection updatedWasteCollection);

        public Task DeleteWasteCollection(WasteCollection updatedWasteCollection);
    }
}