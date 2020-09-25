using BataCMS.Data.Models;
using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.ViewModels
{
    public class EditCategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public string Description { get; set; }

        public IEnumerable<RentalAsset> ItemList { get; set; }
    }
}
