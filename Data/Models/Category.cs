using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public string Description { get; set; }

        public List<RentalAsset> itemList { get; set; }
    }
}
