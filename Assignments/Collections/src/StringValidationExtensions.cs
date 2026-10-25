using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace StaticMethods
{
	public static  class StringValidationExtensions
	{
		public static bool IsvalidEgyptionPhoneNumber(this string phoneNumber)
		{
			if (phoneNumber is null)
			{
				return false;
			}
			var pattern = @"^(?:(?:010|011|012|015)\d{8}|\+20(?:10|11|12|15)\d{8})$";

			return Regex.IsMatch(phoneNumber, pattern);
		}

		public static bool IsvalidEgyptionNationalId(this string nationalId)
		{
			if (nationalId is null)
			{
				return false;
			}
			var pattern = @"^[2-3]\d{13}$";

			return Regex.IsMatch(nationalId, pattern);
		}
	}
}
