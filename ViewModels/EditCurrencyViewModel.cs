using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.ViewModels
{
    public class EditCurrencyViewModel
    {
        public int Id { get; set; }

        [Required]
        public string CurrencyName { get; set; }

        public decimal Rate { get; set; }
    }
}
