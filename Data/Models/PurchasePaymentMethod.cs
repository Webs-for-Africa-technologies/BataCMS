using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Models
{
    public class PurchasePaymentMethod
    {
        public int PurchasePaymentMethodId{ get; set; }

        public int PurchaseId { get; set; }

        public int PaymentMethodId { get; set; }

        public virtual Purchase Purchase { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }

    }
}
