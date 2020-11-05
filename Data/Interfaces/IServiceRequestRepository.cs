using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Interfaces
{
    public interface IServiceRequestRepository
    {
        IEnumerable<ServiceRequest> ODServiceRequests { get; }

        Task<ServiceRequest> AddAsync(ServiceRequest serviceRequest);

        Task<ServiceRequest> GetServiceRequestIdAsync(int serviceId);

        Task UpdateRequestAsync(ServiceRequest serviceRequest);

        void RemoveRequest(ServiceRequest serviceRequest);
    }
}
