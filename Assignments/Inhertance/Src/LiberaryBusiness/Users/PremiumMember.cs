namespace LiberaryBusiness.Users.Member
{
	public class PremiumMember:Member
	{
		private int ReadingValue;
		public int ReadingPoints
		{
			get
			{
				int counter = 0;
				foreach (var loan in ReadOnlyLoans)
				{
					if (loan.Status == LoanStatus.Returned)
					{
						counter++;
					}
				}
				return counter * ReadingValue;
			}
		}

	    public PremiumMember(string FullName, string phoneNumber, int MaxLoans, decimal LateFeeDiscountPercent, int ReadingValue)
							: base(FullName, phoneNumber, MaxLoans, LateFeeDiscountPercent)
		{
			this.ReadingValue = ReadingValue;
		}
	}
}
