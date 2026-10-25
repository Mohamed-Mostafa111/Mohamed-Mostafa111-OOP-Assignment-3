namespace SrpLab;

/// <summary>
/// Loan desk: eligibility arithmetic, required-document lists, and rejection letter prose.
/// </summary>
public sealed class LoanDesk
{
    public decimal RequestedAmount { get; }
    public int CreditScore { get; }
    public int EmploymentMonths { get; }
    public bool HasCollateral { get; }
	private readonly ScoreAccounting ScoreAccounting;
	private readonly DocumentationRequirements requiredDocuments;
	private readonly MessageService messageService;

	public LoanDesk(
		decimal requestedAmount,
		int creditScore,
		int employmentMonths,
		bool hasCollateral,
		ScoreAccounting scoreAccounting, 
		DocumentationRequirements documentationRequirements,
		MessageService MessageService)
    {
        RequestedAmount = requestedAmount;
        CreditScore = creditScore;
        EmploymentMonths = employmentMonths;
        HasCollateral = hasCollateral;
        ScoreAccounting = scoreAccounting;
        requiredDocuments = documentationRequirements;
        messageService = MessageService;
    }

	public string DecisionLetter(string applicantName)
	{
		// Legal/comms wording ≠ underwriting math.
		string message;

		if (ScoreAccounting.IsEligible())
		{
			
			message= $"Dear {applicantName},\nYour request for {RequestedAmount:C} is pre-approved (risk {ScoreAccounting.RiskScore():0}).\n" +
				     $"Please upload: {string.Join("; ", requiredDocuments.RequiredDocuments())}.\n";
		}
		else
		{
			message= $"Dear {applicantName},\nWe are unable to approve {RequestedAmount:C} at this time.\n" +
				   $"Reference risk={ScoreAccounting.RiskScore():0}. You may reapply after improving documentation.\n";
		}

		return messageService.SentMessage(applicantName, message);
	}


}
public sealed class ScoreAccounting
{
    private readonly decimal _requestedAmount;
    private readonly bool _hasCollateral;
    private readonly int _creditScore;
    private readonly int _employmentMonths;

    public ScoreAccounting(decimal requestedAmount, int creditScore, bool hasCollateral, int employmentMonths)
	{
		_requestedAmount = requestedAmount;
		_creditScore = creditScore;
		_hasCollateral = hasCollateral;
		_employmentMonths = employmentMonths;
	}
	public bool IsEligible() => RiskScore() >= 55m && _creditScore >= 580;

	public decimal RiskScore()
	{
		// Risk model will change with risk committee — not with letter templates.
		decimal score = 100m;
		score -= Math.Max(0, 700 - _creditScore) * 0.15m;
		if (_employmentMonths < 6) score -= 20m;
		if (_requestedAmount > 50_000m && !_hasCollateral) score -= 25m;
		if (_requestedAmount > 150_000m) score -= 10m;
		return Math.Clamp(score, 0m, 100m);
	}

	
}
public sealed class DocumentationRequirements
{
	private readonly decimal _requestedAmount;
	private readonly bool _hasCollateral;
	private readonly int _employmentMonths;

	private readonly ScoreAccounting _scoreAccounting;

	public DocumentationRequirements(decimal requestedAmount, bool hasCollateral, int employmentMonths, ScoreAccounting scoreAccounting)
	{
		_requestedAmount = requestedAmount;
		_hasCollateral = hasCollateral;
		_employmentMonths = employmentMonths;
		_scoreAccounting = scoreAccounting;
	}

	public IReadOnlyList<string> RequiredDocuments()
	{
		// Compliance checklist changes with regulation, independently of risk formula.
		var docs = new List<string> { "National ID", "Proof of income (3 months)" };
		if (_requestedAmount > 40_000m) docs.Add("Bank statements (6 months)");
		if (_hasCollateral) docs.Add("Collateral ownership deed");
		if (_employmentMonths < 12) docs.Add("Employer letter");
		if (!_scoreAccounting.IsEligible()) docs.Add("Manual underwriter referral form");
		return docs;
	}
}

public sealed class MessageService
{
	public string SentMessage(string ApplicationName,string Message)
	{
		return Message;
	}
}

public sealed class ReportBuilder
{
	public string UnderwriterCsvRow(string applicationId,int creditScore,int employmentMonths,bool hasCollateral,decimal riskScore,bool isEligible)
	{
		// Analytics export schema is yet another reason to change.
		return $"{applicationId},{creditScore},{employmentMonths},{(hasCollateral ? 1 : 0)},{riskScore:0.00},{(isEligible ? "Y" : "N")}";
	}


}