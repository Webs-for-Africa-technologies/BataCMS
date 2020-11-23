using BataCMS.Data;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Repositories
{
    public class WasteCollectionsRepository : IWasteCollectionsRepository
    {

        private readonly AppDbContext _appDbContext;

        public WasteCollectionsRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
  
        public IEnumerable<WasteCollection> WasteCollections => _appDbContext.WasteCollections;

        public async Task<WasteCollection> AddWasteCollectionAsync(WasteCollection wasteCollection)
        {
            await _appDbContext.WasteCollections.AddAsync(wasteCollection);
            await _appDbContext.SaveChangesAsync();
            return wasteCollection;
        }

        public async Task DeleteWasteCollection(WasteCollection updatedWasteCollection)
        {
            _appDbContext.WasteCollections.Remove(updatedWasteCollection);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<WasteCollection> GetWasteCollectionAsync(int id)
        {
            return await _appDbContext.WasteCollections.FirstOrDefaultAsync(p => p.WasteCollectionId == id);
        }

        public async Task UpdateWasteCollection(WasteCollection updatedWasteCollection)
        {
            _appDbContext.WasteCollections.Update(updatedWasteCollection);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
