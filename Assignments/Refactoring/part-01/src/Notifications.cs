namespace RefactoringLab;

public interface INotification
{
	void Send(string to, string message);
}
public class Notification
{
    private readonly INotification notification;
	private readonly bool isUrgent;
	private readonly DateTime? sendAt;

	public Notification(INotification notification, bool isUrgent = false, DateTime? sendAt = null)
	{
		this.notification = notification;
		this.isUrgent = isUrgent;
		this.sendAt = sendAt;
	}
	public  void Send(string to, string message)
	{
		if (isUrgent)
		{
			message = $"[URGENT] {message}";
		}
		if (sendAt.HasValue)
		{
			to=$"[Scheduled] to send at {sendAt.Value} {to}";
		}
		notification.Send(to, message);
	}
}

public class EmailNotification : INotification
{
    public void Send(string to, string message) =>
        Console.WriteLine($"[email] {to}: {message}");
}

public class SmsNotification : INotification
{
    public void Send(string to, string message) =>
        Console.WriteLine($"[sms] {to}: {message}");
}
public class whatsappNotification : INotification
{
	public void Send(string to, string message) =>
		Console.WriteLine($"[whatsapp] {to}: {message}");
}
