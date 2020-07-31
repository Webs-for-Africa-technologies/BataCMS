using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Models
{
    public class Currency
    {
        public int CurrencyId { get; set; }

        [Required]
        public string CurrencyName { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal Rate { get; set; }

        public bool isCurrent { get; set; }

        internal static void Add(string paymentMethodName, PaymentMethod genre)
        {
            throw new NotImplementedException();
        }
    }
}
