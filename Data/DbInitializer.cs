using BataCMS.Data.Models;
using COHApp.Data.Models;
using BataCMS.Migrations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using RentalAsset = BataCMS.Data.Models.RentalAsset;
using Category = COHApp.Data.Models.Category;

namespace BataCMS.Data
{
    public class DbInitializer
    {
        public static void Seed(IServiceProvider applicationBuilder)
        {
            AppDbContext context = applicationBuilder.GetRequiredService<AppDbContext>();
/*                if (!context.Categories.Any())
            {
                context.Categories.AddRange(Categories.Select(c => c.Value));
            }
*/

            if (!context.PaymentMethods.Any())
            {
                context.PaymentMethods.AddRange(PaymentMethods.Select(c => c.Value));
            }

            if (!context.RentalAssets.Any())
            {
                context.AddRange
                (                   
                );
            }

            context.SaveChanges();
        }

        private static Dictionary<string, Category> categories;
        public static Dictionary<string, Category> Categories
        {
            get
            {
                if (categories == null)
                {
                    var genresList = new Category[]
                    {
                        new Category { CategoryName = "Alcoholic", Description="All alcoholic drinks" },
                        new Category { CategoryName = "Non-alcoholic", Description="All non-alcoholic drinks" },
                        new Category { CategoryName = "Food", Description="Food served by our kitchen" }

                    };

                    categories = new Dictionary<string, Category>();

                    foreach (Category genre in genresList)
                    {
                        categories.Add(genre.CategoryName, genre);
                    }
                }

                return categories;
            }
        }

        private static Dictionary<string, PaymentMethod> paymentMethods;
        public static Dictionary<string, PaymentMethod> PaymentMethods
        {
            get
            {
                if (paymentMethods == null)
                {
                    var genresList = new PaymentMethod[]
                    {
                        new PaymentMethod { PaymentMethodName = "EcoCash"},
                        new PaymentMethod { PaymentMethodName = "Cash"},
                        new PaymentMethod { PaymentMethodName = "Cash(Forex)"},
                        new PaymentMethod { PaymentMethodName = "Debit/Credit Card"},
                    };

                    paymentMethods = new Dictionary<string, PaymentMethod>();

                    foreach (PaymentMethod genre in genresList)
                    {
                        paymentMethods.Add(genre.PaymentMethodName, genre);
                    }
                }
                return paymentMethods;

            }
        }

    }
}
