using System;
using System.Collections.Generic;

namespace BataCMS.Data.Models
{
    public class PurchasedItem
    {
        public int PurchasedItemId { get; set; }

        public int PurchaseId { get; set; }

        public int unitItemId { get; set; }

        public decimal Price { get; set; }

        public int Amount { get; set; }

        public virtual unitItem UnitItem { get; set; }

        public virtual Purchase Purchase { get; set; }


    }
}