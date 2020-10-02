using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using COHApp.Data;

namespace BataCMS.Data.Repositories
{
    public class RentalAssetRepository : IRentalAssetRepository

    {
        private readonly AppDbContext _appDbContext;

        public RentalAssetRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<RentalAsset> rentalAssets => _appDbContext.RentalAssets.Include(c => c.Category);

        public async Task<RentalAsset> GetItemByIdAsync(int unitItemId) => await _appDbContext.RentalAssets.Include(c => c.Images).FirstOrDefaultAsync(p => p.RentalAssetId == unitItemId);


        public async Task<int> DeleteItem(int itemId)
        {
            var unitItem = await _appDbContext.RentalAssets.FindAsync(itemId);


            _appDbContext.RentalAssets.Remove(unitItem);
            var result = await _appDbContext.SaveChangesAsync();
            return result;
        }

        public async Task<RentalAsset> AddAsync(RentalAsset item)
        {
            await _appDbContext.RentalAssets.AddAsync(item);
            await _appDbContext.SaveChangesAsync();
            return item;
        }

        public async Task EditItemAsync(RentalAsset updatedItem)
        {
            _appDbContext.RentalAssets.Update(updatedItem);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task BookAsset(DateTime bookedTill, int assetId)
        {
            RentalAsset rentalAsset = await _appDbContext.RentalAssets.FindAsync(assetId);
            rentalAsset.BookTillDate = bookedTill;
            rentalAsset.IsAvailable = false;
            _appDbContext.RentalAssets.Update(rentalAsset);
            await _appDbContext.SaveChangesAsync(); 
        }

        public async Task EndBooking(int assetId)
        {
            RentalAsset rentalAsset = await _appDbContext.RentalAssets.FindAsync(assetId);
            rentalAsset.BookTillDate = null;
            rentalAsset.IsAvailable = true;
            _appDbContext.RentalAssets.Update(rentalAsset);
            await _appDbContext.SaveChangesAsync();

        }
    }

}
