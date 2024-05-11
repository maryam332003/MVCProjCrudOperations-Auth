using Demo.BLL.Interfaces;
using Demo.BLL.Repository;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Demo.PL.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region Configure Service Which Allow Dependency Injection
            var Builder = WebApplication.CreateBuilder(args);
            Builder.Services.AddControllersWithViews(); // Register Built in MVC Services to the container


            //services.AddScoped<AppDbContext>();


            Builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();     //  Allow DI For DepartmentRepository
            Builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();   //  Allow DI For EmployeeRepository
            Builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();   //  Allow DI For EmployeeRepository
            //services.AddScoped<UserManager<ApplicationUser>>();
            //services.AddScoped<SignInManager<ApplicationUser>>();
            Builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();
            Builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Builder.Configuration.GetConnectionString("DefaultConnection"));

            });

            //services.AddAutoMapper(typeof(MappingProfiles));
            Builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
            Builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
                config.AccessDeniedPath = "/Account/AccessDenied";
            }

                );
            #endregion


            #region Configure HTTP Request Pipline(MiddleWares)

            var app = Builder.Build();
            if (app.Environment.IsDevelopment())
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
            app.UseAuthentication();//who are u
            app.UseAuthorization();//what can you do





            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            #endregion

            app.Run();

        }

    
    }
}
