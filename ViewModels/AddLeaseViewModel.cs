using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.ViewModels
{
    public class AddLeaseViewModel
    {
        public string VendorPhone { get; set; }

        public int RentalAssetId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime leaseFrom { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime leaseTo { get; set; }
    }
    
}


