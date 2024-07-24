using Domain.Models;

namespace Api.Models
{
	public class AccountDTO
	{
		public string Id { get; set; }

		public string? Name { get; set; }

		public string? Description { get; set; }

		public string AccountNumber { get; set; }

		public AccountDTO(Account account)
		{
			this.Id = account.Id.ToString();
			this.Name = account.Name;
			this.Description = account.Description;
			this.AccountNumber = account.AccountNumber;
		}
	}
}
