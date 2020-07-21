using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.ViewModels
{
    public class EditItemViewModel
    {
        public EditItemViewModel()
        {
            Categories = new List<String>();
        }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public bool InStock { get; set; }

        public IList<String> Categories { get; set; }
    }
}
