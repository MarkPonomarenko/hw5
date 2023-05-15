using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using hw6.Data;
using hw6.Data.Entities;
using hw6.Interfaces;
using hw6.Repositories;
using Microsoft.EntityFrameworkCore;

namespace hw6
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //string connectionString = ;
            builder.Services.AddDbContext<ExpensesDbContext>(options => {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IRepository<Expense>, EntityRepository<Expense>>();
            builder.Services.AddScoped<IRepository<Category>, EntityRepository<Category>>();
            builder.Services.AddCloudscribePagination();
            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 5;
                config.IsDismissable = true;
                config.Position = NotyfPosition.TopRight;
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseNotyf();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Category}");

            app.Run();
        }
    }
}