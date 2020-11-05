using System;
using BataCMS.Data;
using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using BataCMS.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Westwind.AspNetCore.LiveReload;
using Microsoft.AspNetCore.Identity;
using BataCMS.ViewModels;
using BataCMS.Infrastructure;
using COHApp.Data.Interfaces;
using COHApp.Data.Repositories;
using COHApp.Data.Models;
using Twilio;
using COHApp.Data.Models.Configuration;


namespace BataCMS
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        readonly IConfigurationRoot _configurationRoot;
        private readonly IConfiguration Configuration;

        public Startup( IHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _configurationRoot = new ConfigurationBuilder().SetBasePath(hostEnvironment.ContentRootPath).AddJsonFile("appsettings.json").Build();
            Configuration = configuration;

        }
        public void ConfigureServices(IServiceCollection services)
        {
            //Server configuration 
            services.AddIdentity<ApplicationUser, IdentityRole>(opt => { 
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 7; 
                opt.Password.RequireNonAlphanumeric = false;
            
            }).AddEntityFrameworkStores<AppDbContext>();
            services.AddIdentityCore<VendorUser>().AddEntityFrameworkStores<AppDbContext>();
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(_configurationRoot.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);


            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IPaymentMethodRepository, PaymentMethodRepository>();
            services.AddTransient<IPurchasePayementMethodRepository, PurchasePaymentMethodRepository>();;
            services.AddTransient<IVendorApplicaitonRepository, VendorApplicationRepository>();
            services.AddTransient<IRentalAssetRepository, RentalAssetRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<ILeaseRepository, LeaseRepository>();
            services.AddTransient<IImageRepository, ImageRepository>();
            services.AddTransient<IActiveLeaseRepository, ActiveLeaseRepository>();
            services.AddTransient<IInvoiceRepository, InvoiceRepository>();
            services.AddTransient<IServiceTypeRepository, ServiceTypeRepository>();
            services.AddTransient<IServiceRequestRepository, ServiceRequestRepository>();
            services.AddTransient<IDispatchedServiceRepository, DispatchedServiceRepository>();




            services.AddSignalR();

            //services.Configure<TwilioAccountDetails>(Configuration.GetSection("TwilioAccountDetails"));

            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddMemoryCache();

            services.AddHttpContextAccessor();


            services.AddSession();

            services.AddLiveReload();

            var accountSid = Configuration["Twilio:AccountSID"];
            var authToken = Configuration["Twilio:AuthToken"];
            TwilioClient.Init(accountSid, authToken);

            services.Configure<TwilioVerifySettings>(Configuration.GetSection("Twilio"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseLiveReload();
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

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SignalServer>("/signalserver");
            });

        }
    }
}
