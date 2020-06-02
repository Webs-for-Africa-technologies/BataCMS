using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.ViewModels
{
    public class CheckoutViewModel
    {
        public Checkout Checkout { get; set;}

        public decimal CheckoutTotal { get; set; }


    }
}
