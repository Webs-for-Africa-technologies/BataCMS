using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Models
{
    public class PurchaseObject
    {
        public int PurchaseAmount { get; set; }
        public decimal Price { get; set; }
        public String ItemName { get; set; }
    }
}
