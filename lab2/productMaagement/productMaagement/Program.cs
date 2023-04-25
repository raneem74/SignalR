using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using productMaagement.Hubs;
using productMaagement.Models;

namespace productMaagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.json", optional: true);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ProductDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            //register depenancy signal Service "DEault setting"
            //server side 
            builder.Services.AddSignalR();

            #region Cors
            builder.Services.AddCors(opt =>

                opt.AddDefaultPolicy(policy =>
                    policy.WithOrigins("http://127.0.0.1:5265").AllowAnyMethod()
                    .AllowAnyHeader().AllowCredentials()
                )
            );
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.MapHub<ProductHub>("ProductHub");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}