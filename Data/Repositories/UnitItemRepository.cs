using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BataCMS.Data.Repositories
{
    public class UnitItemRepository : IUnitItemRepository

    {
        private readonly AppDbContext _appDbContext;

        public UnitItemRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<unitItem> unitItems => _appDbContext.UnitItems.Include(c => c.Category);

        public async Task<unitItem> GetItemByIdAsync(int unitItemId) => await _appDbContext.UnitItems.FirstOrDefaultAsync(p => p.unitItemId == unitItemId);

        //Remove user should not be able to remove unit item to maintain integrity of purchasedItems. 

/*        public async Task<int> DeleteItem(int itemId)
        {
            var unitItem = await _appDbContext.UnitItems.FindAsync(itemId);


                _appDbContext.UnitItems.Remove(unitItem);
                var result = await _appDbContext.SaveChangesAsync();
                return result;

        }*/

        public async Task<unitItem> AddAsync(unitItem item)
        {
            await _appDbContext.UnitItems.AddAsync(item);
            await _appDbContext.SaveChangesAsync();
            return item;
        }

        public async Task EditItemAsync(unitItem updatedItem)
        {
            _appDbContext.UnitItems.Update(updatedItem);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
