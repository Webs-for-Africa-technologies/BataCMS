﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Models
{
    public class CheckoutItem
    {
        public int CheckoutItemId { get; set; }

        public unitItem unitItem { get; set; }

        public int Amount { get; set; }

        public string CheckoutId { get; set; }
    }
}
