using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Interfaces
{
    public interface IVendorApplicaitonRepository
    {
        IEnumerable<VendorApplication> vendorApplications { get; }

        Task<VendorApplication> AddAsync(VendorApplication application);

        Task<VendorApplication> GetApplicationByIdAsync(int applicationId);

        Task UpdateApplicationAsync(VendorApplication application);

    }
}
