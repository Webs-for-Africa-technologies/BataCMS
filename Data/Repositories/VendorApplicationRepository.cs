using BataCMS.Data;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Repositories
{
    public class VendorApplicationRepository : IVendorApplicaitonRepository
    {
        private readonly AppDbContext _appDbContext;

        public VendorApplicationRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<VendorApplication> vendorApplications => _appDbContext.VendorApplications;

        public async Task<VendorApplication> AddAsync(VendorApplication application)
        {
            await _appDbContext.VendorApplications.AddAsync(application);
            await _appDbContext.SaveChangesAsync();
            return application;
        }

    }
}
