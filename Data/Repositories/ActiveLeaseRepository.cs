using BataCMS.Data;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Repositories
{
    public class ActiveLeaseRepository : IActiveLeaseRepository
    {
        private readonly AppDbContext _appDbContext;

        public ActiveLeaseRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddActiveLeaseAsync(ActiveLease activeLease)
        {
            await _appDbContext.AddAsync(activeLease);
            await _appDbContext.SaveChangesAsync();
        }



        public ActiveLease GetActiveLeaseByAssetId(int id)
        {
            return _appDbContext.ActiveLeases.FirstOrDefault(p => p.RentalAssetId == id);
        }

        public async Task RemoveLease(ActiveLease activeLease)
        {
            _appDbContext.ActiveLeases.Remove(activeLease);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
