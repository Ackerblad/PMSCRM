using Microsoft.EntityFrameworkCore;
using PMSCRM.Models;
using PMSCRM.Services;

namespace PMSCRM
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<PmscrmContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddTransient<UserService>();
            builder.Services.AddTransient<TaskService>();
            builder.Services.AddTransient<ProcessService>();
            builder.Services.AddTransient<AreaService>();
            builder.Services.AddTransient<CompanyService>();
            builder.Services.AddTransient<CustomerService>();
            builder.Services.AddTransient<RoleService>();
            builder.Services.AddTransient<TaskProcessAreaService>();
            builder.Services.AddTransient<TaskProcessAreaUserCustomerService>();








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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
