using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BataCMS.Data;
using BataCMS.Data.Interfaces;
using BataCMS.Data.Mocks;
using BataCMS.Data.Models;
using BataCMS.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace BataCMS
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        IConfigurationRoot _configurationRoot;

        public Startup( IHostEnvironment hostEnvironment)
        {
            _configurationRoot = new ConfigurationBuilder().SetBasePath(hostEnvironment.ContentRootPath).AddJsonFile("appsettings.json").Build();

        }
        public void ConfigureServices(IServiceCollection services)
        {


            //Server configuration 

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(_configurationRoot.GetConnectionString("DefaultConnection")));
            services.AddScoped(sp => Checkout.GetCart(sp));

            services.AddTransient<IUnitItemRepository, UnitItemRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IPurchaseRepository, PurchaseRepository>();

            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddMemoryCache();

            services.AddHttpContextAccessor();
            services.AddSession();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
                app.UseStaticFiles();
                app.UseSession();
                app.UseAuthentication();
                //app.UseMvcWithDefaultRoute();
                DbInitializer.Seed(serviceProvider);
                app.UseMvc(routes =>
                {
                    routes.MapRoute(name: "categoryFileter", template: "unitItem/{action}/{category?}", defaults: new { Controller = "unitItem", Action = "List" });
                    routes.MapRoute(name: "default", template: "{controller=Home}/{action=index}/{id?}");
                });
            }

        }
    }
}
