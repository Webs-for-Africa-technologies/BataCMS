using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Models
{
    public class ActiveLease
    {
        public int ActiveLeaseId { get; set; }

        public int RentalAssetId { get; set; }

        public int LeaseId { get; set; }
        
        public virtual RentalAsset RentalAsset { get; set; }

        public virtual Lease Lease { get; set; }
    }
}
