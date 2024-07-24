using System.ComponentModel.DataAnnotations;

namespace Api.Validation
{

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
		AllowMultiple = false)]
	public sealed class InternationalPhoneAttribute : DataTypeAttribute
	{
		private const string AdditionalPhoneNumberCharacters = "-.()";
		private const string ExtensionAbbreviationExtDot = "ext.";
		private const string ExtensionAbbreviationExt = "ext";
		private const string ExtensionAbbreviationX = "x";

		public InternationalPhoneAttribute()
			: base(DataType.PhoneNumber)
		{
			// Set DefaultErrorMessage not ErrorMessage, allowing user to set
			// ErrorMessageResourceType and ErrorMessageResourceName to use localized messages.
			//DefaultErrorMessage = SR.PhoneAttribute_Invalid;
		}

		public InternationalPhoneAttribute(string errorMessage)
			: base(DataType.PhoneNumber)
		{
			ErrorMessage = errorMessage;
		}

		public override bool IsValid(object? value)
		{
			if (value == null)
			{
				return true;
			}

			if (!(value is string valueAsString))
			{
				return false;
			}

			// added to check the phone number contains country code
			if (!(value as string).StartsWith('+'))
			{
				return false;
			}

			ReadOnlySpan<char> valueSpan = valueAsString.Replace("+", string.Empty).AsSpan().TrimEnd();
			valueSpan = RemoveExtension(valueSpan);

			bool digitFound = false;
			foreach (char c in valueSpan)
			{
				if (char.IsDigit(c))
				{
					digitFound = true;
					break;
				}
			}

			if (!digitFound)
			{
				return false;
			}

			foreach (char c in valueSpan)
			{
				if (!(char.IsDigit(c)
					|| char.IsWhiteSpace(c)
					|| AdditionalPhoneNumberCharacters.Contains(c)))
				{
					return false;
				}
			}

			return true;
		}

		private static ReadOnlySpan<char> RemoveExtension(ReadOnlySpan<char> potentialPhoneNumber)
		{
			int lastIndexOfExtension = potentialPhoneNumber
				.LastIndexOf(ExtensionAbbreviationExtDot, StringComparison.OrdinalIgnoreCase);
			if (lastIndexOfExtension >= 0)
			{
				ReadOnlySpan<char> extension = potentialPhoneNumber.Slice(
					lastIndexOfExtension + ExtensionAbbreviationExtDot.Length);
				if (MatchesExtension(extension))
				{
					return potentialPhoneNumber.Slice(0, lastIndexOfExtension);
				}
			}

			lastIndexOfExtension = potentialPhoneNumber
				.LastIndexOf(ExtensionAbbreviationExt, StringComparison.OrdinalIgnoreCase);
			if (lastIndexOfExtension >= 0)
			{
				ReadOnlySpan<char> extension = potentialPhoneNumber.Slice(
					lastIndexOfExtension + ExtensionAbbreviationExt.Length);
				if (MatchesExtension(extension))
				{
					return potentialPhoneNumber.Slice(0, lastIndexOfExtension);
				}
			}

			lastIndexOfExtension = potentialPhoneNumber
				.LastIndexOf(ExtensionAbbreviationX, StringComparison.OrdinalIgnoreCase);
			if (lastIndexOfExtension >= 0)
			{
				ReadOnlySpan<char> extension = potentialPhoneNumber.Slice(
					lastIndexOfExtension + ExtensionAbbreviationX.Length);
				if (MatchesExtension(extension))
				{
					return potentialPhoneNumber.Slice(0, lastIndexOfExtension);
				}
			}

			return potentialPhoneNumber;
		}

		private static bool MatchesExtension(ReadOnlySpan<char> potentialExtension)
		{
			potentialExtension = potentialExtension.TrimStart();
			if (potentialExtension.Length == 0)
			{
				return false;
			}

			foreach (char c in potentialExtension)
			{
				if (!char.IsDigit(c))
				{
					return false;
				}
			}

			return true;
		}
	}
}
