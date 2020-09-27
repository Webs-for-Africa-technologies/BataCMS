using BataCMS.Components;
using BataCMS.Migrations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        [BindNever]
        [ScaffoldColumn(false)]
        public decimal TransactionTotal { get; set; }

        public int VendorUserId { get; set; }

        [Required(ErrorMessage = "Please enter your first name")]
        [StringLength(50)]
        public string ServerName { get; set; }

        [BindNever]
        [ScaffoldColumn(false)]
        public DateTime TransactionDate { get; set; }

        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        public string TransactionNotes { get; set; }

        [Required]
        public string TransactionType { get; set; }

        [Display(Name ="Confirm Delivery")]
        public bool isDelivered { get; set; }

        public Lease Lease { get; set; }


    }
}
