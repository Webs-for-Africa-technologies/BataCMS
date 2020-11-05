using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.ViewModels
{
    public class EditRequestViewModel : CreateODServiceRequestViewModel
    {
        public int ServiceRequestId { get; set; }

        public string ExistingImageURL { get; set; }

        public string Status { get; set; }

        public string RejectMessage { get; set; }
    }
}
