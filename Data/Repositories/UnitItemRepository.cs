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

        public unitItem GetItemById(int unitItemId) => _appDbContext.UnitItems.FirstOrDefault(p => p.unitItemId == unitItemId);

        public async Task<int> DeleteItem(int itemId)
        {
            var unitItem = await _appDbContext.UnitItems.FindAsync(itemId);


                _appDbContext.UnitItems.Remove(unitItem);
                var result = await _appDbContext.SaveChangesAsync();
                return result;

        }

        public  unitItem Add(unitItem item)
        {
            _appDbContext.UnitItems.Add(item);
            _appDbContext.SaveChanges();
            return item;

        }

        public unitItem EditItem(unitItem updatedItem)
        {
            // only save the changes to the repository
            _appDbContext.SaveChanges();
            return  updatedItem;
        }
    }
}
