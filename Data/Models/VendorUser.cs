using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Models
{
    public class VendorUser : ApplicationUser
    {
        public string CardNumber { get; set; }
        public string PhotoIDUrl { get; set; }
    }
}
