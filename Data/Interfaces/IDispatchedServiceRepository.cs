using BataCMS.Migrations;
using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Interfaces
{
    public interface IDispatchedServiceRepository
    {
        IEnumerable<DispatchedService> DispatchedServices { get; }

        public Task<DispatchedService> AddDispatchedServiceAsync(DispatchedService dispatchedService);

        Task<DispatchedService> GetServiceByIdAsync(int id);

        Task UpdateServiceTypeAsync(DispatchedService dispatchedService);

        public Task DeleteServiceTypeAsync(DispatchedService dispatchedService);
    }
}
