using Api.Validation;
using Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.Models
{
	public record ClientUpdateDTO(
		[EmailAddress] string? Email,
		[MaxLength(60)] string? FirstName,
		[MaxLength(60)] string? LastName,
		[MinLength(11), StringLength(11)] string? PersonalId,
		[InternationalPhone("The field MobileNumber is invalid. Check if it contains coutry code started with '+'")] string? MobileNumber,
		[property: JsonConverter(typeof(JsonStringEnumConverter))] Sex? Sex,
		[MaxLength(100)] string? Country,
		[MaxLength(50)] string? City,
		[MaxLength(400)] string? Street,
		[MaxLength(15)] string? ZipCode,
		[MinLength(1)] string[]? AccountNumbers);
}
