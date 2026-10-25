using LiberaryBusiness;
using LiberaryBusiness.Item;
using LiberaryBusiness.Users.Staff;
using System.Data;

//TASK1                                                                               
//var person = new Person("Mohamed", "015156141696");                              |
//var member = new Member();                                                       |
//var Staff = new Staff();                                                         |---->> Not Compiled for protected View
//var Libraryitem = new LiberaryItem();                                            |
//var loan = new Loan(DateOnly.FromDateTime(DateTime.UtcNow), member, Libraryitem);|
//Libraryitem.onLoan();                                                            |

var Ahmed_Student = Liberary.students[0];
var Omar_Student = Liberary.students[1];

Ahmed_Student.Barrow(Liberary.books[1],DateOnly.FromDateTime(DateTime.UtcNow));
//Omar_Student.Barrow(Liberary.books[1]);     |
//Omar_Student.Barrow(Liberary.books[1]);	  |----> throwing new OperationException()
//Omar_Student.Barrow(Liberary.books[1]);	  |
//Omar_Student.Barrow(Liberary.books[1]);	  |

List<staff> staff = new();
staff.AddRange(Liberary.librarians);
staff.AddRange(Liberary.headLibrarians);
staff.AddRange(Liberary.shelfers);

foreach (var employee in staff)
{
	Console.WriteLine($"{employee.FullName} : {employee.MonthlyPay}\n");
}

List<LiberaryItem> liberaryItems = new();
liberaryItems.AddRange(Liberary.books);
liberaryItems.AddRange(Liberary.dvds);
liberaryItems.AddRange(Liberary.magazines);

foreach (var Item in liberaryItems)
{
	Console.WriteLine($"{Item.Title} : {Item.DailyLateFee}\n");
}


var Hany_Liberarian = Liberary.librarians[0];

var Ahmed_PremuimMember = Liberary.premiumMembers[0];

Ahmed_PremuimMember.Barrow(Liberary.dvds[0],DateOnly.FromDateTime(DateTime.UtcNow));


var loan = Ahmed_PremuimMember.ReadOnlyLoans[Ahmed_PremuimMember.ReadOnlyLoans.Count - 1];
Hany_Liberarian.ProcessReturn(loan,new DateOnly(2026,11,3));

Hany_Liberarian.MarkAsLost(loan); // ---> Retun InvalidOperationException Because this loan was Returned
Hany_Liberarian.MarkAsLost(loan); // ---> Retun InvalidOperationException Because this loan was Returned
