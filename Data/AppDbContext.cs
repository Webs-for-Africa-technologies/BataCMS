using BataCMS.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BataCMS.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser> 
    {

         public AppDbContext(DbContextOptions<AppDbContext>options) : base(options)
        {        
        }

        public DbSet<unitItem> UnitItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CheckoutItem> CheckoutItems { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchasedItem> PurchasedItems { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PurchasePaymentMethod> PurchasePaymentMethod { get; set; }

    }
}
