using System;
using System.Collections.Generic;
using System.Text;

namespace RefactoringLab.Part02.Enrollment
{
	public class FacadeEnrollment
	{
		private readonly PaymentGateway _paymentGateway;
		private readonly SeatInventory _seatInventory;
		private readonly InvoiceGenerator _invoiceGenerator;
		private readonly EmailService _emailService;

		public FacadeEnrollment(PaymentGateway paymentGateway, SeatInventory seatInventory, InvoiceGenerator invoiceGenerator, EmailService emailService)
		{
			_paymentGateway = paymentGateway;
			_seatInventory = seatInventory;
			_invoiceGenerator = invoiceGenerator;
			_emailService = emailService;
		}	

		public void EnrollStudent(string studentId, string CourseId, decimal amount)
		{
			_seatInventory.Reserve(CourseId, studentId);
			_paymentGateway.Charge(studentId,amount);
			var InvoiceID = _invoiceGenerator.Create(studentId, amount);
			_emailService.Send(studentId, "Enrollment confirmed", $"Invoice {InvoiceID} for {CourseId}");
		}
	}
}
