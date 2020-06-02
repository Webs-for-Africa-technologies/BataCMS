using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BataCMS.Data.Models
{
    public class unitItem
    {
        [Required]
        public int unitItemId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public bool InStock { get; set; }

        public DateTime DateModified { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

    }
}
    