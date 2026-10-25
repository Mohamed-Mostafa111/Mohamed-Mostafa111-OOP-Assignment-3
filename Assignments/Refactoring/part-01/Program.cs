using RefactoringLab;
using RefactoringLab.Part01.src.ShippingStrategies;

// Shipping Cost Calculation After Refactoring
var shipping = new ShippingCostCalculator();
Console.WriteLine($"Aramex 2kg → {shipping.Calculate(new AramexStrategy(), 2)}");
Console.WriteLine($"FedEx 2kg  → {shipping.Calculate(new FedExStrategy(), 2)}");
Console.WriteLine();

// Order Processing After Refactoring
var processor = new OrderProcessor(new SqlOrderRepository(), new SmtpEmailSender());
processor.Process(1001, "customer@example.com");
Console.WriteLine();

// Messages After Refactoring
Notification EmailNote = new Notification(new EmailNotification(), true,DateTime.UtcNow);

Notification SmsNote = new Notification(new SmsNotification(), true);

Notification WhatsappNote = new Notification(new whatsappNotification(), true);

EmailNote.Send("customer@example.com", "Your order ships tomorrow");                                    

SmsNote.Send("+201000000000", "OTP 4821");

WhatsappNote.Send("+201002683652", "Your order has been received");
