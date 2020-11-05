using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Models
{
    public class DispatchedService
    {
        public int DispatchedServiceId { get; set; }

        public string ApplicationUserId { get; set; }

        public int ServiceRequestId { get; set; }

        public DateTime DispatchTime { get; set; }

        public virtual ServiceRequest ServiceRequest { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
