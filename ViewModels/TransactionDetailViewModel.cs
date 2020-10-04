using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.ViewModels
{
    public class TransactionDetailViewModel
    {
        public Transaction Transaction { get; set; }
        public RentalAsset RentalAsset { get; set; }
        public ApplicationUser VendorUser { get; set; }
        public Lease Lease { get; set; }

    }
}
