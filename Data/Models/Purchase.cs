﻿using BataCMS.Components;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Models
{
    public class Purchase
    {
        public int PurchaseId { get; set; }

        [BindNever]
        [ScaffoldColumn(false)]
        public decimal PurchasesTotal { get; set; }


        [Required(ErrorMessage = "Please enter your first name")]
        [StringLength(50)]
        public string ServerName { get; set; }

        [BindNever]
        [ScaffoldColumn(false)]
        public DateTime PurchaseDate { get; set; }

        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        public string PurchaseNotes { get; set; }

        [Required]
        [Display(Name = "Table Numeber")]
        public string DeliveryLocation { get; set; }

        [Display(Name ="Confirm Delivery")]
        public bool isDelivered { get; set; }


        public List<PurchasedItem> PurchasedItems { get; set; }

        public List<PaymentMethod> PaymentMethods { get; set; }



    }
}
