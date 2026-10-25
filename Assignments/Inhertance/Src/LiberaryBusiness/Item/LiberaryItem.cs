using System;
using System.Collections.Generic;
using System.Text;

namespace LiberaryBusiness.Item
{
	public class LiberaryItem
	{
		public string CatalogNumber { get; }
		public string Title { get; }
		public int LoanPeriodDays { get; }
		public decimal DailyRental{ get; private set; }
		private decimal BaseLateFee;
		public decimal DailyLateFee => DailyRental * BaseLateFee;
		public bool IsWithdrawn { get; private set; }
		public bool IsAvailable { get; private set; }
		
		protected LiberaryItem(string CatalogNumber,string Title,int LoanDays,decimal DailyRental,decimal BaseLateFee)
		{
			this.CatalogNumber = CatalogNumber;
			this.Title = Title;
			LoanPeriodDays = LoanDays;
			this.DailyRental = DailyRental;
			this.BaseLateFee = BaseLateFee;
			IsAvailable = true;
		}
		
		public void Withdraw()
		{
			IsWithdrawn = true;
			IsAvailable = false;
		}
		public void Restore()
		{
			IsWithdrawn = false;
			IsAvailable = true;
		}
		public void onLoan()
		{
			IsAvailable = false;
		}
		public void ChangeDailyLateFee(decimal newDailyfee)
		{
			BaseLateFee = newDailyfee;
		}
		public void ChangeDailRental(decimal DailyRental)
		{
			if (DailyRental<=0)
			{
				throw new ArgumentException("Dailty renatal must to be more than zeero");
			}
			this.DailyRental = DailyRental;
		}
	}
}
