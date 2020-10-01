using BataCMS.Data.Interfaces;
using COHApp.Data.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BataCMS.Data.Models
{
    public class RentalAsset
    {
        [Required]
        public int RentalAssetId { get; set; }

        [Required]
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime DateModified { get; set; }

        public DateTime BookTillDate { get; set; }

        public int CategoryId { get; set; }

        public string Location { get; set; }

        [Required]
        public virtual Category Category { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public List<Image> Images { get; set; }

    }
}
    