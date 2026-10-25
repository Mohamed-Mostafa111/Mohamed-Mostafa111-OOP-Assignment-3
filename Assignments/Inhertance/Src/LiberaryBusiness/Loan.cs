using LiberaryBusiness.Item;
using LiberaryBusiness.Users.Member;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Text;

namespace LiberaryBusiness
{
	public class Loan
	{
		public int Id { get; }
		private static int counter = 0;
		public DateOnly BorrowDate { get; }
		public LiberaryItem Item;
		public Member Member;
		public DateOnly DueDate => BorrowDate.AddDays(Item.LoanPeriodDays);
		public DateOnly ReturnDate { get; private set; }
		public LoanStatus Status { get; private set; }

		public decimal LateFee
		{
			get
			{
				if(ReturnDate>DueDate)
				{
					int delayDayes = ReturnDate.DayNumber - DueDate.DayNumber;
					return delayDayes * Item.DailyLateFee;
				}
				return 0m;
			}
		}
		public bool IsActive { get; set; }
		public Loan(DateOnly BorrowDate, Member member, LiberaryItem item)
		{
			Id = counter++;
			Item = item;
			Member = member;
			Status = LoanStatus.Borrowed;
			IsActive = true;
			this.BorrowDate = BorrowDate;
		}
		public void Return(DateOnly returnDate)
		{
			if (returnDate<BorrowDate)
			{
				throw new ArgumentException("Return date cannot be earlier than borrow date.");
			}
			if(Status!=LoanStatus.Borrowed)
			{
				throw new InvalidOperationException("This loan is not active.");
			}

			ReturnDate = returnDate;
			IsActive = false;
			Status = LoanStatus.Returned;
		}
		public void MarkAsLost()
		{
			if (Status != LoanStatus.Borrowed)
			{
				throw new InvalidOperationException("This loan is not active.");
			}
			Status = LoanStatus.Lost;
		}
	}
}