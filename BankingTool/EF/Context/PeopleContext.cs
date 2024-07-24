using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EF.Context
{
	public class PeopleContext : DbContext
	{
		public PeopleContext(DbContextOptions<PeopleContext> options) : base(options)
		{

		}

		public DbSet<Account> Accounts { get; set; }

		public DbSet<Client> Clients { get; set; }

	}
}