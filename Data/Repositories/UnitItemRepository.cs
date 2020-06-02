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
    }
}
