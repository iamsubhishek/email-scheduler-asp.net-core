using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Email_Scheduler.Controllers;
using Email_Scheduler.Interface;
using Email_Scheduler.InterfaceImplementation;
using Email_Scheduler.Models;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Email_Scheduler
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddHangfire(config => config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                                 .UseSimpleAssemblyNameTypeSerializer()
                                 .UseDefaultTypeSerializer()
                                 .UseMemoryStorage());
            services.AddHangfireServer();
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddSingleton<IMailService, MailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager,
            IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseHangfireDashboard();
            backgroundJobClient.Enqueue(() => Console.WriteLine("Hello Hangfire job!"));
            recurringJobManager.AddOrUpdate(
                "Run every minute",
                () => serviceProvider.GetService<IMailService>().SendEmail(),
                "* * * * *"
                );
        }
    }
}
