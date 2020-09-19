using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Models
{
    public class Lease
    {
        public int LeaseId{ get; set; }

        public int VendorUserId { get; set; }

        public int RentalAssetId { get; set; }

        public virtual RentalAsset RentalAsset { get; set; }

    }
}
