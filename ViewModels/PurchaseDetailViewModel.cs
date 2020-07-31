
using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.ViewModels
{
    public class PurchaseDetailViewModel
    {
        public int PurchaseId { get; set; }
        public List<PurchaseObject> PurchasedItems { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public Purchase Purchase { get; set; }
        public Currency Currency { get; set; }
        

    }
}
