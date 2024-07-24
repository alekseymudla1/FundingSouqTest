using NUlid;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
	public class Account
	{
		public Ulid Id { get; set; }

		[MaxLength(50)]
		public string? Name { get; set; }

		[MaxLength(100)]
		public string? Description { get; set; }

		[Required]
		[MaxLength(50)]
		public string AccountNumber { get; set; }
	}
}
