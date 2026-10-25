namespace RefactoringLab;

public class OrderProcessor
{
    private readonly IOrderRepository _orderRepository;
    private readonly ISender _emailSender;
	public OrderProcessor(IOrderRepository orderRepository, ISender emailSender)
    {
        _orderRepository = orderRepository;
        _emailSender = emailSender;
    }

	public void Process(int orderId, string customerEmail)
    {
        _orderRepository.Save(orderId, DateTime.Now);
        _emailSender.Send(customerEmail, $"Order {orderId} confirmed at {DateTime.Now}");
    }
}

public class SqlOrderRepository:IOrderRepository
{
    public void Save(int orderId, DateTime processedAt) =>
        Console.WriteLine($"[SQL] save order {orderId} @ {processedAt:O}");
}

public class SmtpEmailSender:ISender
{
    public void Send(string to, string body) =>
        Console.WriteLine($"[SMTP] to={to} body={body}");
}

public interface IOrderRepository
{
	void Save(int orderId, DateTime processedAt);
}
public interface ISender
{
	void Send(string to, string body);
}