using BataCMS.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BataCMS.Data
{
    public class DbInitializer
    {
        public static void Seed(IServiceProvider applicationBuilder)
        {
            AppDbContext context = applicationBuilder.GetRequiredService<AppDbContext>();

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(Categories.Select(c => c.Value));
            }


            if (!context.PaymentMethods.Any())
            {
                context.PaymentMethods.AddRange(PaymentMethods.Select(c => c.Value));
            }

            if (!context.UnitItems.Any())
            {
                context.AddRange
                (
                    new unitItem
                    {
                        Name = "Beer",
                        Price = 7.95M,
                        Category = Categories["Alcoholic"],
                        InStock = true,
                        ImageUrl = "/Content/Images/beer.PNG",
                    },
                    new unitItem
                    {
                        Name = "Rum & Coke",
                        Price = 12.95M,
                        Category = Categories["Alcoholic"],
                        InStock = true,
                        ImageUrl = "/Content/Images/rum and coke.PNG",
                    },
                    new unitItem
                    {
                        Name = "Tequila ",
                        Price = 12.95M,
                        Category = Categories["Alcoholic"],
                        InStock = true,
                        ImageUrl = "~/Content/Images/tequila.PNG",

                    },
                    new unitItem
                    {
                        Name = "Wine ",
                        Price = 16.75M,
                        Category = Categories["Alcoholic"],
                        InStock = true,
                        ImageUrl = "~/Content/Images/wine.PNG",
                    },
                    new unitItem
                    {
                        Name = "Burger and Chips",
                        Price = 17.95M,
                        Category = Categories["Food"],
                        InStock = true,
                        ImageUrl = "~/Content/Images/burger and chips.PNG",

                    },
                    new unitItem
                    {
                        Name = "Whiskey with Ice",
                        Price = 15.95M,
                        Category = Categories["Alcoholic"],
                        InStock = false,
                        ImageUrl = "~/Content/Images/whiskey with ice.PNG",

                    },
                    new unitItem
                    {
                        Name = "Jägermeister",
                        Price = 15.95M,
                        Category = Categories["Alcoholic"],
                        InStock = false,
                        ImageUrl = "~/Content/Images/jag.PNG",

                    },
                    new unitItem
                    {
                        Name = "Champagne",
                        Price = 15.95M,
                        Category = Categories["Alcoholic"],
                        InStock = false,
                        ImageUrl = "~/Content/Images/champagne.PNG",

                    },
                    new unitItem
                    {
                        Name = "Piña colada ",
                        Price = 15.95M,
                        Category = Categories["Alcoholic"],
                        InStock = false,
                        ImageUrl = "~/Content/Images/pina colada.PNG",

                    },
                    new unitItem
                    {
                        Name = "White Russian",
                        Price = 15.95M,
                        Category = Categories["Alcoholic"],
                        InStock = false,
                        ImageUrl = "~/Content/Images/white russian.PNG",

                    },
                    new unitItem
                    {
                        Name = "Long Island Iced Tea",
                        Price = 15.95M,
                        Category = Categories["Alcoholic"],
                        InStock = false,
                        ImageUrl = "~/Content/Images/long island.PNG",

                    },
                    new unitItem
                    {
                        Name = "Vodka",
                        Price = 15.95M,
                        Category = Categories["Alcoholic"],
                        InStock = false,
                    },
                    new unitItem
                    {
                        Name = "Gin and tonic",
                        Price = 15.95M,
                        Category = Categories["Alcoholic"],
                    },
                    new unitItem
                    {
                        Name = "Cosmopolitan",
                        Price = 15.95M,
                        Category = Categories["Alcoholic"],
                        InStock = false,
                    },
                    new unitItem
                    {
                        Name = "Fish and Chips ",
                        Price = 12.95M,
                        Category = Categories["Food"],
                        InStock = true,
                    },
                    new unitItem
                    {
                        Name = "Water ",
                        Price = 12.95M,
                        Category = Categories["Non-alcoholic"],
                        InStock = true,
                    },
                    new unitItem
                    {
                        Name = "Coffee ",
                        Price = 12.95M,
                        Category = Categories["Non-alcoholic"],
                        InStock = true,
                    },
                    new unitItem
                    {
                        Name = "Kvass",
                        Price = 12.95M,
                        Category = Categories["Non-alcoholic"],
                        InStock = true,
                    }
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
