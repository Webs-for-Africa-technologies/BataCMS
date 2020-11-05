using BataCMS.Data;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace COHApp.Data.Repositories
{
    public class ServiceTypeRepository : IServiceTypeRepository
    {
        private readonly AppDbContext _appDbContext;

        public ServiceTypeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<ServiceType> ServiceTypes => _appDbContext.ServiceTypes;

        public async Task DeleteServiceTypeAsync(ServiceType serviceType)
        {
            _appDbContext.ServiceTypes.Remove(serviceType);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<ServiceType> GetServiceTypeByIdAsync(int id)
        {
              return await _appDbContext.ServiceTypes.FirstOrDefaultAsync(p => p.ServiceTypeId == id);
        }

        public async Task UpdateServiceTypeAsync(ServiceType updatedServiceType)
        {
            _appDbContext.ServiceTypes.Update(updatedServiceType);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<ServiceType> AddServiceTypeAsync(ServiceType serviceType)
        {
            await _appDbContext.ServiceTypes.AddAsync(serviceType);
            await _appDbContext.SaveChangesAsync();
            return serviceType;
        }
    }
}
