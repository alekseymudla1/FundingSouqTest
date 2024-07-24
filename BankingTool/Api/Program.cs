using Api.Cache;
using Api.DataSeed;
using Api.Middlewares;
using Api.Models;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using EF.Context;
using EF.Repositories;
using Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Api
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Host.UseSerilog((context, configuration) =>
				configuration.ReadFrom.Configuration(context.Configuration));


			builder.Services.AddDbContext<PeopleContext>(options =>
			{
				options.UseInMemoryDatabase(Constants.DatabaseName);
			});

			builder.Services.AddAuthorization();
			builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);
			builder.Services.AddDbContext<AppIdentityDbContext>(options =>
			{
				options.UseInMemoryDatabase(Constants.IdentityDatabaseName);
			});

			builder.Services.AddIdentityCore<IdentityUser>()
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<AppIdentityDbContext>()
				.AddApiEndpoints();

			builder.Services.AddTransient<IClientRepository, ClientRepository>();
			builder.Services.AddSingleton<IQueryCache<QueryObject>, QueryCache>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();
			app.UseMiddleware<ExceptionMiddleware>();

			app.MapControllers();
			app.MapIdentityApi<IdentityUser>();
			app.SeedData();
			app.SeedIdentityData();
			app.Run();
		}
	}
}
