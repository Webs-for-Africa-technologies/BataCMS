using BataCMS.Data;
using BataCMS.Data.Models;
using COHApp.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace COHApp.Data.Repositories
{
    public class LeaseRepository : ILeaseRepository
    {

        private readonly AppDbContext _appDbContext;
        public LeaseRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Lease> AddLeaseAsync(Lease lease)
        {
            await _appDbContext.Leases.AddAsync(lease);
            await _appDbContext.SaveChangesAsync();
            return lease;
        }

        public async Task<Lease> GetLeaseById(int leaseId)
        {
            return await _appDbContext.Leases.FirstOrDefaultAsync(p => p.LeaseId == leaseId);

        }
    }
}
