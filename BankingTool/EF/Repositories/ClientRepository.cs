using Domain.Interfaces.Repositories;
using Domain.Models;
using EF.Context;
using Microsoft.EntityFrameworkCore;
using NUlid;

namespace EF.Repositories
{
	public class ClientRepository : IClientRepository
	{
		private readonly PeopleContext _dbContext;
		public ClientRepository(PeopleContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<Client>> GetClientsAsync(FilterOptions filterOptions, SortOptions sortOptions, int offset, int limit)
		{
			// filtering inverted just because I use InMemory provider here
			// using any sql provider filtering is invoked before ToListAsync() call
			var query = await _dbContext.Clients.AsQueryable().AsNoTracking().ToListAsync();

			var result = query.Where(c => GetStringExpression(filterOptions.Email, c.Email))
				.Where(c => GetStringExpression(filterOptions.FirstName, c.FirstName))
				.Where(c => GetStringExpression(filterOptions.LastName, c.LastName))
				.Where(c => GetStringExpression(filterOptions.PersonalId, c.PersonalId))
				.Where(c => GetStringExpression(filterOptions.MobileNumber, c.MobileNumber))
				.Where(c => filterOptions.Sex.HasValue ? c.Sex.Equals(filterOptions.Sex.Value) : true);

			if (sortOptions.SortDirection == SortDirection.Descending)
			{
				result = AddSortDesc(result, sortOptions.SortField);
			}
			else
			{
				result = AddSort(result, sortOptions.SortField);
			}

			return result.Skip(offset).Take(limit);
		}

		public async Task<Client> GetClientAsync(Ulid id)
		{
			return await _dbContext.Clients
				.Include(c => c.Address)
				.Include(c => c.Accounts)
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id.Equals(id));
		}

		public async Task<Client> GetClientTrackedAsync(Ulid id)
		{
			return await _dbContext.Clients
				.Include(c => c.Address)
				.Include(c => c.Accounts)
				.FirstOrDefaultAsync(c => c.Id.Equals(id));
		}

		public async Task<Client> CreateClientAsync(Client client, Address address, string[] accountNumbers)
		{
			client.Address = address;
			var accountsInDb = await _dbContext.Accounts.Where(a => accountNumbers.Contains(a.AccountNumber)).ToListAsync();
			var accountsToCreate = accountNumbers
				.Where(a =>
				!accountsInDb.Exists(acc => acc.AccountNumber.Equals(a, StringComparison.OrdinalIgnoreCase)))
				.Select(a => new Account()
				{
					Id = Ulid.NewUlid(),
					AccountNumber = a
				});
			client.Accounts = accountsInDb.Concat(accountsToCreate).ToArray();

			_dbContext.AddRange(accountsToCreate);
			_dbContext.Add(address);
			_dbContext.Add(client);

			await _dbContext.SaveChangesAsync();
			return client;
		}

		public async Task<Client> UpdateClientAsync(Client client, string[] accountNumbers)
		{
			if (accountNumbers is not null && accountNumbers.Length > 0)
			{
				var accountsInDb = await _dbContext.Accounts.Where(a => accountNumbers.Contains(a.AccountNumber)).ToListAsync();
				var accountsToCreate = accountNumbers
					.Where(a =>
					!accountsInDb.Exists(acc => acc.AccountNumber.Equals(a, StringComparison.OrdinalIgnoreCase)))
					.Select(a => new Account()
					{
						Id = Ulid.NewUlid(),
						AccountNumber = a
					});

				var clientAccounts = accountsInDb.Concat(accountsToCreate).ToArray();
				client.Accounts = clientAccounts;
			}

			await _dbContext.SaveChangesAsync();
			return client;
		}

		private bool GetStringExpression(string filter, string fieldValue)
		{
			return !string.IsNullOrWhiteSpace(filter) ? fieldValue.Contains(filter) : true;
		}

		private IEnumerable<Client> AddSort(IEnumerable<Client> query, string sortField)
		{
			return sortField switch
			{
				"email" => query.OrderBy(c => c.Email),
				"firstname" => query.OrderBy(c => c.FirstName),
				"lastname" => query.OrderBy(c => c.LastName),
				"personalid" => query.OrderBy(c => c.PersonalId),
				"mobilenumber" => query.OrderBy(c => c.MobileNumber),
				_ => query.OrderBy(c => c.Id)
			};
		}

		private IEnumerable<Client> AddSortDesc(IEnumerable<Client> query, string sortField)
		{
			return sortField switch
			{
				"email" => query.OrderByDescending(c => c.Email),
				"firstname" => query.OrderByDescending(c => c.FirstName),
				"lastname" => query.OrderByDescending(c => c.LastName),
				"personalid" => query.OrderByDescending(c => c.PersonalId),
				"mobilenumber" => query.OrderByDescending(c => c.MobileNumber),
				_ => query.OrderByDescending(c => c.Id)
			};
		}
	}
}
