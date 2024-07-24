using NUlid;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
	public class Client
	{
		public Ulid Id { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[MaxLength(100)]
		public string FirstName { get; set; }

		[Required]
		[MaxLength(100)]
		public string LastName { get; set; }

		[Required]
		[StringLength(11)]
		public string PersonalId { get; set; }

		[Required]
		[DataType(DataType.PhoneNumber)]
		[Phone]
		public string MobileNumber { get; set; }

		[Required]
		public Sex Sex { get; set; }

		public Ulid AddressId { get; set; }

		[Required]
		[MaxLength(400)]
		public Address Address { get; set; }

		public IEnumerable<Account> Accounts { get; set; }

	}
}
