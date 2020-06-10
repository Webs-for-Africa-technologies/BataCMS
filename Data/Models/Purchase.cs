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
        public int PurchaseID { get; set; }
        
        public decimal PurchasesTotal { get; set; }


        [Required(ErrorMessage = "Please enter your first name")]
        [Display(Name = "First name")]
        [StringLength(50)]
        public string ServerName { get; set; }

        [BindNever]
        [ScaffoldColumn(false)]
        public DateTime PurchaseDate { get; set; }

        public List<PurchasedItem> PurchasedItems { get; set; } 

        public virtual PaymentMethod PaymentMethod { get; set; }
    }
}
