using System.Runtime.CompilerServices;

namespace SrpLab;

/// <summary>
/// Support ticket: SLA clocks, NLP-ish priority from free text, and public reply templates.
/// </summary>
public  class SupportTicket
{
    public string Id { get; }
    public string Subject { get; private set; }
    public string Body { get; private set; }
    public DateTimeOffset OpenedAt { get; }
    public string Priority { get; private set; } = "P3";
    private readonly Formatter _formatter = new Formatter();
	private readonly SLA _sla;
	private readonly DraftPublicReply _draftPublicReply;

	public SupportTicket(string id, string subject, string body, DateTimeOffset openedAt)
    {
        Id = id;
        Subject = subject;
        Body = body;
        OpenedAt = openedAt;
        _sla = new SLA(Priority, OpenedAt);
        _draftPublicReply = new DraftPublicReply(Priority, Id, _sla);
        _formatter.RecalculatePriorityFromText(Priority, Subject, Body);
    }
	public string DraftPublicReply(string agentName)
	{
		return _draftPublicReply.CreatePublicReply(agentName);
	}
}
public sealed class Formatter
{

	public void AppendCustomerMessage(string text,ref string Body,ref string Priority,string Subject)
	{
		Body += "\n---\n" + text;
		RecalculatePriorityFromText( Priority, Subject, Body);
	}

	public void RecalculatePriorityFromText(string Priority, string Subject, string Body)
	{
		// Keyword heuristics will churn with support playbooks; SLA math will not.
		var blob = (Subject + " " + Body).ToLowerInvariant();
		if (blob.Contains("down") || blob.Contains("outage") || blob.Contains("cannot login"))
			Priority = "P1";
		else if (blob.Contains("urgent") || blob.Contains("asap") || blob.Contains("blocked"))
			Priority = "P2";
		else
			Priority = "P3";
	}
}

public class SLA
{
    private readonly string Priority;
    private readonly DateTimeOffset OpenedAt;

	public SLA(string priority, DateTimeOffset openedAt)
	{
		Priority = priority;
		OpenedAt = openedAt;		
	}
	public DateTimeOffset SlaDeadline()
    {
        // Operational SLA policy — different stakeholders than reply wording.
        var hours = Priority switch
        {
            "P1" => 4,
            "P2" => 24,
            _ => 72
        };
        return OpenedAt.AddHours(hours);
    }

    public bool IsBreached(DateTimeOffset now) => now > SlaDeadline();

}

public sealed class DraftPublicReply
{
	private readonly string Priority;
	private readonly string Id;
	private readonly SLA sLA;
	public DraftPublicReply(string priority, string id, SLA sLA)
	{
		Priority = priority;
		Id = id;
		this.sLA = sLA;
	}
	public string CreatePublicReply(string agentName)
	{
		// Tone/templates owned by CX — not by SLA engineering.
		var apology = Priority == "P1" ? "We are treating this as a critical incident." : "Thanks for reaching out.";
		return $"Hi,\n{apology}\nTicket {Id} is with {agentName}. Next update before {sLA.SlaDeadline():u}.\n";
	}

	public string InternalEscalationBlurb()
	{
		return $"ESCALATE {Id} priority={Priority} breachAt={sLA.SlaDeadline():u} keywords-scanned=yes";
	}
}