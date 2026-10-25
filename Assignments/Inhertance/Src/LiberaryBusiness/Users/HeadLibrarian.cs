using LiberaryBusiness.Item;

namespace LiberaryBusiness.Users.Staff.Staff
{
	public class HeadLibrarian:staff
	{
		public HeadLibrarian(string FullName, string PhoneNumber,
				DateOnly hiredate, decimal monthlySalary,
				decimal responsibilityAllowance) : base(FullName, PhoneNumber, hiredate, monthlySalary, responsibilityAllowance)
		{

		}
		public void Restore(LiberaryItem item)
		{
			item.Restore();
			Console.WriteLine($"this Book {item.Title} has been restored by HeadLibrarian {FullName} and is available right now");
		}
		public void withdraw(LiberaryItem item)
		{
			item.Withdraw();
			Console.WriteLine($"this Book {item.Title} has been withdrawn by HeadLibrarian {FullName} and is un available right now");

		}
		public void ChangeLateFee(LiberaryItem item,decimal latefee)
		{
			if (latefee<0)
			{
				throw new ArgumentException("Fee Can't be Nagative");
			}
			item.ChangeDailyLateFee(latefee);
		}
	}
}
