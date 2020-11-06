
using BataCMS.Data;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace COHApp.Data.Repositories
{
    public class DispatchedServiceRepository : IDispatchedServiceRepository
    {

        private readonly AppDbContext _appDbContext;

        public DispatchedServiceRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<DispatchedService> DispatchedServices => _appDbContext.DispatchedServices.Include(p => p.ServiceRequest).ThenInclude(p => p.ServiceType);

        public async Task<DispatchedService> AddDispatchedServiceAsync(DispatchedService dispatchedService)
        {
            await _appDbContext.DispatchedServices.AddAsync(dispatchedService);
            await _appDbContext.SaveChangesAsync();
            return dispatchedService;
        }

        public Task DeleteServiceTypeAsync(DispatchedService dispatchedService)
        {
            throw new NotImplementedException();
        }

        public async Task<DispatchedService> GetServiceByIdAsync(int id)
        {
            return await _appDbContext.DispatchedServices.Include(p => p.ApplicationUser).Include(p => p.ServiceRequest).ThenInclude(p => p.ServiceType).FirstOrDefaultAsync(p => p.DispatchedServiceId == id);
        }

        public Task UpdateServiceTypeAsync(DispatchedService dispatchedService)
        {
            throw new NotImplementedException();
        }
    }
}
