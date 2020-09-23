using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Models
{
    public class VendorApplication
    {
        public int VendorApplicationId { get; set; }

        public string ApplicantName { get; set; }

        [Required]
        public string ApplicantId { get; set; }
        public string IdProofUrl { get; set; }
        public string ResidencyProofUrl { get; set; }
        public string Status { get; set; }
        public DateTime ApplicationDate { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
