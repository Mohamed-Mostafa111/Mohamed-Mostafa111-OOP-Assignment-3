using LiberaryBusiness.Item;

namespace LiberaryBusiness.Users.Member
{
	public class Member : Person
	{
		private readonly List<Loan> Loans;
		public  IReadOnlyList<Loan> ReadOnlyLoans => Loans;
		public int MaxMumLoans { get; }
		public decimal LateFeeDiscountPercent { get; }
		private int LoanCounter;
		public bool IsBlocked { get; private set; }
		public Member(string FullName, string PhoneNumber, int MaxLoans, decimal LateFeeDiscountPercent) : base(FullName, PhoneNumber)
		{
			Loans = new List<Loan>();
			MaxMumLoans = MaxLoans;
			this.LateFeeDiscountPercent = LateFeeDiscountPercent;
			LoanCounter = 0;
		}

		public void Barrow(LiberaryItem item,DateOnly BorrowDate)
		{
			if (LoanCounter>=MaxMumLoans)
			{
				throw new InvalidOperationException("You have reached the maximum limit for loans at this Month");
			}
			if (!item.IsAvailable)
			{
				throw new ArgumentException($"sorry mr {FullName} this item at another Loan");
			}
			if(item.IsWithdrawn)
			{
				throw new ArgumentException("this item is withdrawn by HeadLiberarian");
			}
			var Loan = new Loan(BorrowDate, this, item);
			item.onLoan();
			Loans.Add(Loan);
			LoanCounter++;
			
		}
		public void Block()
		{
			IsBlocked = true;
		}

	}
}
