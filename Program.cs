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

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddControllersWithViews();

            // Console.WriteLine(Environment.GetEnvironmentVariable("FirebaseMathApp"));

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseSession();

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
