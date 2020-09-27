using BataCMS.Data.Models;
using COHApp.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace BataCMS.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser> 
    {

         public AppDbContext(DbContextOptions<AppDbContext>options) : base(options)
        {        
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>()
                .HasIndex(b => b.CategoryName).IsUnique(true);
            modelBuilder.Entity<ApplicationUser>().HasAlternateKey(p => new { p.IDNumber, p.PhoneNumber}); 

        }


        public DbSet<Category> Categories { get; set; }
        public DbSet<RentalAsset> RentalAssets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Lease> Leases { get; set; }
        public DbSet<VendorUser> VendorUsers { get; set; }
        public DbSet<VendorApplication> VendorApplications { get; set; }

    }
}
