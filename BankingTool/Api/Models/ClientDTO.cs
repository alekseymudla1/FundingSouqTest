using Domain.Models;
using System.Text.Json.Serialization;

namespace Api.Models
{
	public class ClientDTO
	{
		public string Id { get; set; }

		public string Email { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string PersonalId { get; set; }

		public string MobileNumber { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public Sex Sex { get; set; }

		public AddressDTO Address { get; set; }

		public IEnumerable<AccountDTO> Accounts { get; set; }

		// I prefer using constructors and methods instead of Automappers
		// but company codestyle is more important for me
		public ClientDTO(Client client)
		{
			this.Id = client.Id.ToString();
			this.Email = client.Email;
			this.FirstName = client.FirstName;
			this.LastName = client.LastName;
			this.PersonalId = client.PersonalId;
			this.MobileNumber = client.MobileNumber;
			this.Sex = client.Sex;
			this.Address = client.Address is not null ? new AddressDTO(client.Address) : null;
			this.Accounts = client.Accounts is not null ? client.Accounts.Select(acc => new AccountDTO(acc)) : [];
		}
	}
}
