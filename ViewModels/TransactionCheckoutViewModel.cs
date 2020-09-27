using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.ViewModels
{
    public class TransactionCheckoutViewModel
    {
        public string TransactionType { get; set; }
        public int LeaseId { get; set; }
        public string TransactionNotes { get; set; }

    }
}
