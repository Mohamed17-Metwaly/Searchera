using Microsoft.EntityFrameworkCore;
using Searchera.Models;
using System;

namespace Searchera
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<JobBoardSystemContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("JobBoardSystem")/*"Server=.;Database=Crud_Operation;Encrypt=false;Trusted_Connection=true"*/);
            });
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();
            app.UseStaticFiles();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Review}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
