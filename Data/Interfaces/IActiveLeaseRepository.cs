using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Interfaces
{
    public interface IActiveLeaseRepository
    {
        ActiveLease GetActiveLeaseByAssetId(int id);

         Task AddActiveLeaseAsync(ActiveLease activeLease);

        public Task RemoveLease(ActiveLease activeLease);
    }
}
