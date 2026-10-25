namespace LiberaryBusiness.Users.Staff
{
	public class Librarian:staff
	{
		public Librarian(string FullName, string PhoneNumber,
						DateOnly hiredate, decimal monthlySalary,
						decimal responsibilityAllowance) : base(FullName, PhoneNumber,hiredate,monthlySalary,responsibilityAllowance)
		{

		}

		public void ProcessReturn(Loan loan,DateOnly ReturnDate)
		{
			if (loan == null || !loan.IsActive)
				throw new InvalidOperationException("This loan record is invalid or already closed.");

			loan.Return(ReturnDate);

			if (loan.ReturnDate>loan.DueDate)
			{
				int delayedDayes = ReturnDate.DayNumber - loan.DueDate.DayNumber;

				Console.WriteLine($"[Librarian] Book returned late. Penalty applied: {loan.LateFee} for {delayedDayes} DelayedDayes");
			}
		}
		public void MarkAsLost(Loan loan)
		{
			loan.MarkAsLost();
		}
	}
}
