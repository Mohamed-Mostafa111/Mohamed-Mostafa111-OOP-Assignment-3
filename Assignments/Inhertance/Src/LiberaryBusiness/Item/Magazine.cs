namespace LiberaryBusiness.Item
{
	public class Magazine:LiberaryItem
	{
		public Magazine(string CatalogNumber, string Title, int LoanDays,decimal DailyRental, decimal BaseLateFee) : base(CatalogNumber, Title, LoanDays, DailyRental, BaseLateFee)
		{
		}
	}
}
