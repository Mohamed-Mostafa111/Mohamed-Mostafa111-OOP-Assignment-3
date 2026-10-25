namespace LiberaryBusiness.Item
{
	public class Book:LiberaryItem
	{

		public Book(string CatalogNumber, string Title, int LoanDays,decimal DailyRental ,decimal BaseLateFee):base(CatalogNumber,Title,LoanDays, DailyRental, BaseLateFee)
		{
		}
	}
}
