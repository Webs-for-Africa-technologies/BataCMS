using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Models
{
    public class VendorUser : ApplicationUser
    {
        public int VendorUserId { get; set; }
        public int CardNumber { get; set; }
        public string PhotoIDUrl { get; set; }
    }
}
