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

            modelBuilder.Entity<RentalAsset>().Property(e => e.OptionFormData).HasConversion(v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }), v => JsonConvert.DeserializeObject<string>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            modelBuilder.Entity<CheckoutItem>().Property(e => e.selectedOptions).HasConversion(v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }), v => JsonConvert.DeserializeObject<string>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            modelBuilder.Entity<PurchasedItem>().Property(e => e.selectedOptionData).HasConversion(v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }), v => JsonConvert.DeserializeObject<string>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

        }


        public DbSet<Category> Categories { get; set; }
        public DbSet<RentalAsset> RentalAssets { get; set; }
        public DbSet<CheckoutItem> CheckoutItems { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchasedItem> PurchasedItems { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PurchasePaymentMethod> PurchasePaymentMethod { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<VendorUser> VendorUsers { get; set; }



    }
}
