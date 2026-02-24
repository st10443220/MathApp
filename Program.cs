using MathApp.Data;
using Microsoft.EntityFrameworkCore;

namespace MathApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<MathAppContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("MathAppContext")
                        ?? throw new InvalidOperationException(
                            "Connection string 'MathAppContext' not found."
                        )
                )
            );

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

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
                    pattern: "{controller=Math}/{action=Calculate}/{id?}"
                )
                .WithStaticAssets();

            app.Run();
        }
    }
}
