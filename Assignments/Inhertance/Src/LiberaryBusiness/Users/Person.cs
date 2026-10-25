using System;
using System.Collections.Generic;
using System.Text;

namespace LiberaryBusiness.Users
{
	public class Person
	{
		public int Id { get; }
		public string FullName { get; }
		public string PhoneNumber { get; }
		private static int IdCounter = 0;
		protected Person(string fullName, string phoneNumber)
		{
			Id = IdCounter++;
			FullName = fullName;
			PhoneNumber = phoneNumber;
		}
	}
}
