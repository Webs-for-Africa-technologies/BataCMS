using BataCMS.Data;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Repositories
{
    public class VendorApplicationRepository : IVendorApplicaitonRepository
    {
        private readonly AppDbContext _appDbContext;
        //private readonly IHubContext<SignalServer> _hubContext;

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

        public async Task<VendorApplication> GetApplicationByIdAsync(int applicationId)
        {           
            return await _appDbContext.VendorApplications.Include(p => p.ApplicationUser).FirstOrDefaultAsync(p => p.VendorApplicationId == applicationId);
        }

        public async Task UpdateApplicationAsync(VendorApplication application)
        {
            _appDbContext.VendorApplications.Update(application);
            await _appDbContext.SaveChangesAsync();

            //var ApplicationCount = (vendorApplications.Where(p => p.Status == "Pending")).Count();
            //await _hubContext.Clients.All.SendAsync("updatePurchase", OrderCount);
        }
    }
}
