using BataCMS.Data;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Repositories
{
    public class ServiceRequestRepository : IServiceRequestRepository
    {
        private readonly AppDbContext _appDbContext;
        //private readonly IHubContext<SignalServer> _hubContext;

        public ServiceRequestRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<ServiceRequest> ODServiceRequests => _appDbContext.ServiceRequests;

        public async Task<ServiceRequest> AddAsync(ServiceRequest serviceRequest)
        {
            await _appDbContext.ServiceRequests.AddAsync(serviceRequest);
            await _appDbContext.SaveChangesAsync();
            return serviceRequest;
        }

        public async Task<ServiceRequest> GetServiceRequestIdAsync(int requestId)
        {
            return await _appDbContext.ServiceRequests.Include(p => p.ApplicationUser).FirstOrDefaultAsync(p => p.ServiceRequestId == requestId);
        }

        public void RemoveRequest(ServiceRequest serviceRequest)
        {
            _appDbContext.ServiceRequests.Remove(serviceRequest);
             _appDbContext.SaveChanges();
        }

        public async Task UpdateRequestAsync(ServiceRequest serviceRequest)
        {
            _appDbContext.ServiceRequests.Update(serviceRequest);
            await _appDbContext.SaveChangesAsync();

            //var ApplicationCount = (vendorApplications.Where(p => p.Status == "Pending")).Count();
            //await _hubContext.Clients.All.SendAsync("updatePurchase", OrderCount);
        }
    }
}
