using System;
using System.Collections.Generic;

namespace BataCMS.Data.Models
{
    public class PurchasedItem
    {
        public int PurchasedItemId { get; set; }

        public int TransactionId { get; set; }

        public int unitItemId { get; set; }

        public decimal Price { get; set; }

        public int Amount { get; set; }

        public string selectedOptionData { get; set; }

        public virtual RentalAsset RentalAsset { get; set; }


    }
}