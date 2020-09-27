using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Models
{
    public class Lease
    {
        public int LeaseId{ get; set; }

        public string UserId { get; set; }

        public int RentalAssetId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime leaseFrom { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime leaseTo { get; set; }

        public virtual RentalAsset RentalAsset { get; set; }


    }
}
