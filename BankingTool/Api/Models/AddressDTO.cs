using Domain.Models;

namespace Api.Models
{
	public class AddressDTO
	{
		public string Id { get; set; }

		public string Country { get; set; }

		public string City { get; set; }

		public string Street { get; set; }

		public string ZipCode { get; set; }

		public AddressDTO(Address address)
		{
			this.Id = address.Id.ToString();
			this.Country = address.Country;
			this.City = address.City;
			this.Street = address.Street;
			this.ZipCode = address.ZipCode;
		}
	}
}
