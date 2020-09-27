using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Interfaces
{
    public interface ILeaseRepository
    {
        public Task<Lease> AddLeaseAsync(Lease lease);

        Task<Lease> GetLeaseById(int leaseId);

    }
}
