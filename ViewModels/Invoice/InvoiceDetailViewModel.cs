using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.ViewModels
{
    public class InvoiceDetailViewModel
    {
        public int InvoiceId { get; set; }

        public string RentalAssetName { get; set; }

        public string RentalAssetLocation { get; set; }

        public decimal RentalAssetPrice { get; set; }

        public DateTime LeaseFrom { get; set; }

        public DateTime LeaseTo { get; set; }

        public decimal AmountPaid { get; set; }
    }
}
