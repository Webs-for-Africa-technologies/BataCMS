using BataCMS.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser> 
    {

         public AppDbContext(DbContextOptions<AppDbContext>options) : base(options)
        {        
        }

        public DbSet<unitItem> UnitItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CheckoutItem> CheckoutItems { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchasedItem> PurchasedItems { get; set; }


    }
}
