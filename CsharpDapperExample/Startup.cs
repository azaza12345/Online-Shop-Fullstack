using System.Reflection;
using CsharpDapperExample.Models;
using CsharpDapperExample.Repository;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CsharpDapperExample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IRepository<Product>, SqlProductRepository>();
            services.AddControllersWithViews();

            services.AddFluentMigratorCore()
                .ConfigureRunner(configure =>
                    configure.AddPostgres()
                        .WithGlobalConnectionString(Configuration.GetValue<string>("DBInfo:ConnectionString"))
                        .ScanIn(Assembly.GetExecutingAssembly()).For.All())
                .AddLogging(configure => configure.AddFluentMigratorConsole());
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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

            using var scope = app.ApplicationServices.CreateScope();
            var migrator = scope.ServiceProvider.GetService<IMigrationRunner>();
            migrator.MigrateUp();
        }
    }
}