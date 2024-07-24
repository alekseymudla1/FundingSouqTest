using Domain.Models;
using EF.Context;
using Microsoft.AspNetCore.Identity;
using NUlid;

namespace Api.DataSeed
{
	public static class WebAppDataSeedEstention
	{
		public static void SeedData(this WebApplication app)
		{
			using var scope = app.Services.CreateScope();
			using var context = scope.ServiceProvider.GetRequiredService<PeopleContext>();
			{
				try
				{
					context.Database.EnsureCreated();

					var accounts = new Account[]
					{
						new Account() { Id = new Ulid("01J3E56NF824CJSJFXRDKSF3MH"), AccountNumber = "GB48BARC20035327795261" },
						new Account() { Id = new Ulid("01J3E5RVBMGFGA1STQ7399RS2G"), AccountNumber = "GB66BARC20039531817229" },
						new Account() { Id = new Ulid("01J3E5SNZ9H7AC71RDMGEWE8MD"), AccountNumber = "GB49BARC20037882116322" },
						new Account() { Id = new Ulid("01J3E67CQKZHM8GNA5HY89M483"), AccountNumber = "GB24BARC20040439937126" }
					};

					context.AddRange(
						accounts
					);

					context.AddRange(
					[
						new Client() {
							Id = new Ulid("01J3FWDA54414DN9BC4N35TN6G"),
							Email = "a_einstein@app.com",
							FirstName = "Albert",
							LastName = "Einstein",
							PersonalId = "000612-2394",
							MobileNumber = "+44 079 0292 2932",
							Sex = Sex.Male,
							Address = new Address() {
								Id = new Ulid("01J3FX8EJDDEV2F662C7515MAN"),
								Country = "UK",
								City = "London",
								Street = "Oxford st.",
								ZipCode = "W1D 1BS"
							},
							Accounts = [ accounts[0] ]
						},
						new Client() {
							Id = new Ulid("01J3FXYMR8WRCW5HN4QSVTD866"),
							Email = "n_tesla@app.com",
							FirstName = "Nicola",
							LastName = "Tesla",
							PersonalId = "810326-9299",
							MobileNumber = "+44 078 0403 4586",
							Sex = Sex.Male,
							Address = new Address() {
								Id = new Ulid("01J3FYTNCVJ1XY6ABK81TGT0FK"),
								Country = "UK",
								City = "London",
								Street = "Cambridge st.",
								ZipCode = "ON N6H 1N7"
							},
							Accounts = [ accounts[1] ]
						}
					]);
					context.SaveChanges();
				}
				catch (Exception)
				{

					throw;
				}
			}
		}

		public static async void SeedIdentityData(this WebApplication app)
		{
			using (var scope = app.Services.CreateScope())
			{
				var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
				var roles = new[] { "admin", "user" };

				foreach (var role in roles)
				{
					if (!await roleManager.RoleExistsAsync(role))
					{
						await roleManager.CreateAsync(new IdentityRole(role));
					}
				}

				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

				var email = "admin@app.com";
				var password = "Password!1";
				if ((await userManager.FindByEmailAsync("admin@app.com")) is null)
				{
					var user = new IdentityUser();
					user.Email = email;
					user.UserName = email;
					await userManager.CreateAsync(user, password);

					await userManager.AddToRoleAsync(user, "admin");
				}
			}
			//using var context = scope.ServiceProvider.GetRequiredService<PeopleContext>();
			//{
			//	try
			//	{
			//		context.Database.EnsureCreated();

			//		context.AddRange(
			//		[
			//			new Identity.Role() { Name = "Admin"},
			//			new Identity.Role() { Name = "User" }
			//		]);

			//		var admin = new Identity.User()
			//		{
			//			UserName = "Admin",
			//			Email = "admin@app.com",
			//		};
			//		var adminPwd = new PasswordHasher<Identity.User>().HashPassword(admin, "Password1!");
			//		admin.PasswordHash = adminPwd;

			//		context.AddRange(
			//		[
			//			admin
			//		]);
			//		context.SaveChanges();
			//	}
			//	catch (Exception)
			//	{

			//		throw;
			//	}
			//}
		}
	}
}
