using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Auth0.AspNetCore.Authentication;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;
using DALL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using CORE.Services;
using CORE.Interfaces.IRepositories;
using DALL.Repositories;

namespace WEB
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<ISpotOccupationRepository, SpotOccupationRepository>();

            // Register PaymentService
            builder.Services.AddScoped<PaymentService>();


            // Auth0 configuratie
            builder.Services.AddAuth0WebAppAuthentication(options =>
			{
				options.Domain = builder.Configuration["Auth0:Domain"];
				options.ClientId = builder.Configuration["Auth0:ClientId"];
			});

			// Voeg dit toe als je MVC controllers en views gebruikt
			builder.Services.AddControllersWithViews(); // Belangrijk voor MVC

			// Als je applicatie ook autorisatiebeleid gebruikt
			builder.Services.AddAuthorization();

			// Database initialization
			SqlConnectionStringBuilder conBuilder = new SqlConnectionStringBuilder();

			builder.Services.AddDbContext<GarageContext>(options =>
				{
					// options.UseSqlServer("Data Source=10.0.3.10;Database=mediaan_parking_garage;User ID=SA;Password=Software01!;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
					options.UseInMemoryDatabase("MockGarageDB");
				});

			// Build
			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
