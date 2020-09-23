using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Models
{
    public class VendorApplication
    {
        public int VendorApplicationId { get; set; }
        public string ApplicantName { get; set; }
        public string IdProofUrl { get; set; }
        public string ResidencyProofUrl { get; set; }
        public string Status { get; set; }
        public DateTime ApplicationDate { get; set; }
    }
}
