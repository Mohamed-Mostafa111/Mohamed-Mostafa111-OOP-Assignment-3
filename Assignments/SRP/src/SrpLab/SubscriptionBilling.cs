using System.Reflection.Metadata.Ecma335;

namespace SrpLab;

/// <summary>
/// Subscription billing: proration math, invoice number minting, and dunning email bodies.
/// </summary>
public sealed class SubscriptionBilling
{
    private static int _invoiceSeq = 1000;
    public string CustomerId { get; }
    public decimal MonthlyPrice { get; }
    public DateOnly PeriodStart { get; }
    public DateOnly PeriodEnd { get; }
    public int FailedPayments { get; private set; }

	private readonly InvoiceCalCulations invoice;
	private readonly MailArrangement mail;
	private readonly ProtationMath protation;

	public SubscriptionBilling(string customerId, decimal monthlyPrice, DateOnly periodStart, DateOnly periodEnd)
	{
		CustomerId = customerId;
		MonthlyPrice = monthlyPrice;
		PeriodStart = periodStart;
		PeriodEnd = periodEnd;
		invoice = new InvoiceCalCulations(_invoiceSeq,periodStart,FailedPayments);
		protation = new ProtationMath(periodStart,periodEnd,monthlyPrice);
		mail = new MailArrangement (protation,invoice,FailedPayments);
	}

	public void RegisterFailedPayment()
	{
		invoice.RegisterFailedPayment();
	}

	public string DunningEmail(string CustomerName,DateOnly asOf)
	{
		return mail.DunningEmail(CustomerName,asOf);
	}

}


public sealed class InvoiceCalCulations
{
    private static  int _invoiceSeq;
	private DateOnly  PeriodStart;
    private  int FailedPayments;

	public InvoiceCalCulations(int invoiceSeq, DateOnly periodStart, int failedPayments)
	{
		_invoiceSeq = invoiceSeq;
		PeriodStart = periodStart;
		FailedPayments = failedPayments;
	}

	public string NextInvoiceNumber()
	{
		// Numbering scheme / fiscal prefixes — ops concern, not pricing.
		var n = ++_invoiceSeq;
		return $"INV-{PeriodStart:yyyyMM}-{n:D5}";
	}

	public void RegisterFailedPayment() => FailedPayments++;
}

public sealed class ProtationMath
{
    private readonly DateOnly PeriodStart;
    private readonly DateOnly PeriodEnd;
    private readonly decimal MonthlyPrice;

	public ProtationMath(DateOnly periodStart, DateOnly periodEnd, decimal monthlyPrice)
	{
		PeriodStart = periodStart;
		PeriodEnd = periodEnd;
		MonthlyPrice = monthlyPrice;
	}

	public decimal Prorate(DateOnly activeFrom)
	{
		// Finance calendar rules change independently of email copy.
		if (activeFrom <= PeriodStart) return MonthlyPrice;
		if (activeFrom >= PeriodEnd) return 0m;
		var totalDays = PeriodEnd.DayNumber - PeriodStart.DayNumber;
		if (totalDays <= 0) return MonthlyPrice;
		var used = PeriodEnd.DayNumber - activeFrom.DayNumber;
		return Math.Round(MonthlyPrice * used / totalDays, 2);
	}
}

public sealed class MailArrangement
{
	private readonly DateOnly PeriodStart;
	private readonly InvoiceCalCulations invoiceCalCulations;
	private readonly ProtationMath protationMath;
	private readonly int FailedPayments;

	public MailArrangement(ProtationMath protationMath, InvoiceCalCulations invoiceCalCulations, int failedPayments)
	{
		this.protationMath = protationMath;
		this.invoiceCalCulations = invoiceCalCulations;
		FailedPayments = failedPayments;
	}

	public string DunningEmail(string customerName, DateOnly asOf)
	{
		// Collections tone & legal boilerplate ≠ proration formula.
		var amount = protationMath.Prorate(PeriodStart);
		var invoice =invoiceCalCulations.NextInvoiceNumber(); // side-effect while composing mail — nasty on purpose
		var severity = FailedPayments switch
		{
			<= 1 => "friendly reminder",
			2 => "second notice",
			_ => "final notice before suspension"
		};
		return $"Subject: {severity} {invoice}\nHi {customerName},\nBalance {amount:C} as of {asOf:o} ({FailedPayments} failures).\n";
	}
}

public sealed class LargerJournalLine
{

	private readonly int CustomerId;
	private readonly InvoiceCalCulations invoiceCal;
	private readonly ProtationMath portablePdb;

	public LargerJournalLine(int customerId, InvoiceCalCulations invoiceCal, ProtationMath portablePdb)
	{
		CustomerId = customerId;
		this.invoiceCal = invoiceCal;
		this.portablePdb = portablePdb;
	}

	public string LedgerJournalLine(DateOnly activeFrom)
	{
		// Accounting export format is another axis of change.
		return $"{CustomerId},{invoiceCal.NextInvoiceNumber()},{portablePdb.Prorate(activeFrom):0.00},AR-SUB";
	}

}
