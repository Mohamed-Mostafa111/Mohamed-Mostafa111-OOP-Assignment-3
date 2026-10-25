using RefactoringLab.Part02.Enrollment;
using RefactoringLab.Part02.Reports;

Console.WriteLine("=== Reports ===");
var outDir = Path.Combine(Path.GetTempPath(), "/refactoring-lab-part02");
Directory.CreateDirectory(outDir);

new CsvReportExporter().Export(Path.Combine(outDir, "report.csv"));
new JsonReportExporter().Export(Path.Combine(outDir, "report.json"));
new TextReportExporter().Export(Path.Combine(outDir, "report.txt"));

Console.WriteLine($"Wrote reports to {outDir}");
Console.WriteLine();


//After Applaing Facade Pattern Refactoring
Console.WriteLine("=== Enrollment ===");
var studentId = "S100";
var courseId = "CS201";
var amount = 1500m;

var Enrollment = new FacadeEnrollment(new PaymentGateway(), new SeatInventory(), new InvoiceGenerator(), new EmailService());
Enrollment.EnrollStudent(studentId, courseId, amount);
