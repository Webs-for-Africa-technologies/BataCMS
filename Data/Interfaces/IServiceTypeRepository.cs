using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Interfaces
{
    public interface IServiceTypeRepository
    {

        IEnumerable<ServiceType> ServiceTypes { get; }

        public Task<ServiceType> AddServiceTypeAsync(ServiceType serviceType);

        Task<ServiceType> GetServiceTypeByIdAsync(int id);

        Task UpdateServiceTypeAsync(ServiceType serviceType);

        public Task DeleteServiceTypeAsync(ServiceType serviceType);
    }
}
