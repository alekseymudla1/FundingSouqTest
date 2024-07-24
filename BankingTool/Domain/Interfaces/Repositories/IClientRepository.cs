using Domain.Models;
using NUlid;

namespace Domain.Interfaces.Repositories
{
	public interface IClientRepository
	{
		Task<IEnumerable<Client>> GetClientsAsync(FilterOptions filterOptions, SortOptions sortOptions, int offset, int limit);

		Task<Client> GetClientAsync(Ulid id);

		Task<Client> GetClientTrackedAsync(Ulid id);

		Task<Client> CreateClientAsync(Client client, Address address, string[] accountNumbers);

		Task<Client> UpdateClientAsync(Client client, string[] accountNumbers);
	}
}
