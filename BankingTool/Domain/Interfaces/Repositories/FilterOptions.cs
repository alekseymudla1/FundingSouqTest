using Domain.Models;

namespace Domain.Interfaces.Repositories
{
	public class FilterOptions
	{
		public string? Email { get; set; } = null;
		public string? FirstName { get; set; } = null;

		public string? LastName { get; set; } = null;

		public string? PersonalId { get; set; } = null;

		public string? MobileNumber { get; set; } = null;

		public Sex? Sex { get; set; } = null;
	}
}
