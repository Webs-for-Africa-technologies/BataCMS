using BataCMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.ViewModels
{
    public class EditApplicationViewModel : CreateVendorApplicationViewModel
    {
        public int VendorApplicationId { get; set; }
        
        public string ExitingIdProofURL { get; set; }

        public string ExistingResidencyProofURL { get; set; }

        public string Status { get; set; }

        public string RejectMessage { get; set; }
        
    }
}
