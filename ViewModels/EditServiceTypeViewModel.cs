using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.ViewModels
{
    public class EditServiceTypeViewModel
    {
        public int Id { get; set; }

        [Required]
        public string ServiceTypeName { get; set; }

        public string Description { get; set; }

        public decimal Pricing { get; set; }

    }
}
