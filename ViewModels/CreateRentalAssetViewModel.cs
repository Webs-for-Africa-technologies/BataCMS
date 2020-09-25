using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.ViewModels
{
    public class CreateRentalAssetViewModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }


        [Required]
        public string Category { get; set; }

        public List<IFormFile> Images { get; set; }
    }
}
