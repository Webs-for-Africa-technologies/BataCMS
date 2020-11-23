using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.ViewModels
{
    public class EditMessageViewModel
    {
        public int Id { get; set; }

        [Required]
        public string MessageTitle { get; set; }

        public string MessageContent { get; set; }
    }
}
