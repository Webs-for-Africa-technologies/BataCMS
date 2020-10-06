using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }

        public int RentalAssetId { get; set; }

        public string ApplicationId { get; set; }

        public DateTime LeaseFrom { get; set; }

        public DateTime LeaseTo { get; set; }

        public decimal AmountPaid { get; set; }

        public virtual RentalAsset RentalAsset { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
