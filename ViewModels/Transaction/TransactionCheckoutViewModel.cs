using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.ViewModels
{
    public class TransactionCheckoutViewModel
    {
        public string TransactionType { get; set; }
        public string ServerId { get; set; }
        public int LeaseId { get; set; }
        public string TransactionNotes { get; set; }
        public decimal TransactionTotal { get; set; }
        public double RentalDuration { get; set; }
        public decimal AssetPricing { get; set; }

    }
}
