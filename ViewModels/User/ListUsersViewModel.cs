using BataCMS.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.ViewModels
{
    public class ListUsersViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }

        public IQueryable<IdentityRole> Roles { get; set; }

    }
}
