using Microsoft.VisualBasic;
using System.Security.Cryptography.X509Certificates;

namespace LiberaryBusiness.Users.Staff
{
	public class staff : Person
	{
		public DateOnly Hiredate { get; }
		public decimal MonthlySalary { get; private set; }
		private decimal responsibilityAllowance;
		public decimal MonthlyPay { get { return MonthlySalary + responsibilityAllowance;} }

		protected staff(string FullName, string PhoneNumber,
						DateOnly hiredate, decimal monthlySalary,
						decimal responsibilityAllowance) : base(FullName, PhoneNumber)
		{
			Hiredate = hiredate;
			MonthlySalary = monthlySalary;
			this.responsibilityAllowance = responsibilityAllowance;
		}
		public void GiveRise(decimal Precentage)
		{
			if (Precentage <= 0)
				throw new ArgumentException("Percenatge Must to be more than Zero");
			MonthlySalary += (MonthlySalary * (Precentage / 100));
		}
		public override string ToString()
		{
			return $"Name: {FullName}\n" +
				   $" Phone: {PhoneNumber}\n" +
				   $" Hire Date: {Hiredate}\n" +
				   $"Monthly Salary: {MonthlySalary:C}\n" +
				   $"responsibilityAllowance:{responsibilityAllowance}\n" +
				   $" Total Pay: {MonthlyPay:C}\n" +
				   $"*------------------------------------------------------------*";
		}
	}
}
