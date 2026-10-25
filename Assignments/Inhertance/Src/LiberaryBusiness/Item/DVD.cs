namespace LiberaryBusiness.Item
{
	public class DVD:LiberaryItem
	{
		public DVD(string CatalogNumber, string Title, int LoanDays,decimal DailyRental, decimal BaseLateFee) : base(CatalogNumber, Title, LoanDays, DailyRental, BaseLateFee)
		{
		}
	}
}
