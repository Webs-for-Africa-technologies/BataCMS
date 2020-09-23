
using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.ViewModels
{
    public class ViewApplicationViewModel
    {
        public int VendorApplicationId { get; set; }
        public ApplicationUser Applicant { get; set; }
        public string IdProofUrl { get; set; }
        public string ResidenceProof { get; set; }
        public DateTime ApplicationDate { get; set; }

    }
}
