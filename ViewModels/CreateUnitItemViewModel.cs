using BataCMS.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.ViewModels
{
    public class CreateUnitItemViewModel
    {
        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public bool InStock { get; set; }

        [Required]
        public string Category { get; set; }

        public IFormFile Image { get; set; }
    }
}
