using Domain.Interfaces.Repositories;
using Domain.Models;

namespace Api.Models
{
	public class QueryObject
	{
		public string? Email { get; set; } = null;

		public string? FirstName { get; set; } = null;

		public string? LastName { get; set; } = null;

		public string? PersonalId { get; set; } = null;

		public string? MobileNumber { get; set; } = null;

		public string? Sex { get; set; } = null;

		public string? SortBy { get; set; } = null;

		public string? SortDirection { get; set; } = null;

		public FilterOptions GetFilterOptions()
		{
			return new FilterOptions()
			{
				Email = this.Email,
				FirstName = this.FirstName,
				LastName = this.LastName,
				PersonalId = this.PersonalId,
				MobileNumber = this.MobileNumber,
				Sex = Enum.TryParse<Sex>(this.Sex, true, out var sex) ? sex : null
			};
		}

		public SortOptions GetSortOptions()
		{
			return new SortOptions()
			{
				SortDirection = Enum.TryParse<SortDirection>(this.SortDirection, out var direction) ? direction : Domain.Interfaces.Repositories.SortDirection.Ascending,
				SortField = SortBy?.ToLower()
			};
		}

		public override bool Equals(object? obj)
		{
			if (obj is QueryObject query)
			{
				return FieldEquals(this.Email, query.Email) &&
					FieldEquals(this.FirstName, query.FirstName) &&
					FieldEquals(this.LastName, query.LastName) &&
					FieldEquals(this.PersonalId, query.PersonalId) &&
					FieldEquals(this.MobileNumber, query.MobileNumber) &&
					FieldEquals(this.Sex, query.Sex) &&
					FieldEquals(this.SortBy, query.SortBy) &&
					FieldEquals(this.SortDirection, query.SortDirection);
			}

			return false;
		}

		private bool FieldEquals(object? item1, object? item2)
		{
			return (item1 is null && item2 is null) || (item1 is not null) && item1.Equals(item2);
		}
	}
}
