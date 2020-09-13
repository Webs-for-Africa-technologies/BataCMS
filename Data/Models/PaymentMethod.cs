using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Models
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }

        [Required]
        public string PaymentMethodName { get; set; }
        public decimal AmountPaid { get; set; }
        public bool isConfirmed { get; set; }
    }
}
