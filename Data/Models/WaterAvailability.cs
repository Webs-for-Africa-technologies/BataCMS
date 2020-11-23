using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Models
{
    public class WaterAvailability
    {
        public int WaterAvailabilityId { get; set; }

        public string ServiceArea { get; set; }

        public DateTime DateModified { get; set; }

        public bool Mon { get; set; }
        public bool Tue { get; set; }
        public bool Wed { get; set; }
        public bool Thur { get; set; }
        public bool Fri { get; set; }
        public bool Sat { get; set; }
        public bool Sun { get; set; }
    }
}
