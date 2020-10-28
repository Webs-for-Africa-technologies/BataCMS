using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.ViewModels
{
    public class EditUserViewModel
    {

        public EditUserViewModel()
        {
            Roles = new List<string>();
        }

        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string IdNumber { get; set; }

        public string Number { get; set; }

        [Display(Name = "Add to Role")]
        public string AddedRole { get; set; }

        [Display(Name = "Remove From Role")]
        public string RemovedRole { get; set; }

        public IList<string> Roles { get; set; }

    }
}
