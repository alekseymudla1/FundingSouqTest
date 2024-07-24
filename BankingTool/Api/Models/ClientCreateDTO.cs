using Api.Validation;
using Domain.Models;
using NUlid;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.Models
{
	public record ClientCreateDTO(
			[Required][EmailAddress] string Email,
			[Required][MaxLength(60)] string FirstName,
			[Required][MaxLength(60)] string LastName,
			[Required][MinLength(11), StringLength(11)] string PersonalId,
			[Required][InternationalPhone("The field MobileNumber is invalid. Check if it contains coutry code started with '+'")] string MobileNumber,
			[Required][property: JsonConverter(typeof(JsonStringEnumConverter))] Sex Sex,
			[Required][MaxLength(100)] string Country,
			[Required][MaxLength(50)] string City,
			[Required][MaxLength(400)] string Street,
			[Required][MaxLength(15)] string ZipCode,
			[Required, MinLength(1)] string[] AccountNumbers
		)
	{
		public Client ToClient()
		{
			return new Client()
			{
				Id = Ulid.NewUlid(),
				Email = this.Email,
				FirstName = this.FirstName,
				LastName = this.LastName,
				PersonalId = this.PersonalId,
				MobileNumber = this.MobileNumber,
				Sex = this.Sex//Enum.TryParse<Sex>(this.Sex, out var result) ? result : Domain.Models.Sex.Male
			};
		}

		public Address Address()
		{
			return new Address()
			{
				Id = Ulid.NewUlid(),
				Country = this.Country,
				City = this.City,
				Street = this.Street,
				ZipCode = this.ZipCode
			};
		}
	}
}
