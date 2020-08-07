using BataCMS.Components;
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


        public List<PurchasedItem> PurchasedItems { get; set; }

        public List<PaymentMethod> PaymentMethods { get; set; }



    }
}
