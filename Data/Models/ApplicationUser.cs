using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BataCMS.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IDNumber { get; set; }
        public string PhysicalAddress { get; set; }
    }
}
