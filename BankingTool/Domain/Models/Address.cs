using NUlid;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
	public class Address
	{
		public Ulid Id { get; set; }

		[Required]
		[MaxLength(100)]
		public string Country { get; set; }

		[Required]
		[MaxLength(50)]
		public string City { get; set; }

		[Required]
		[MaxLength(400)]
		public string Street { get; set; }

		[Required]
		[MaxLength(15)]
		public string ZipCode { get; set; }
	}
}
