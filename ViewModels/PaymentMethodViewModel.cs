using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.ViewModels
{
    public class PaymentMethodViewModel
    {
        public  IOrderedEnumerable<PaymentMethod> PaymentMethods { get; set; }

        public int AmountPaid { get; set; }


    }
}
