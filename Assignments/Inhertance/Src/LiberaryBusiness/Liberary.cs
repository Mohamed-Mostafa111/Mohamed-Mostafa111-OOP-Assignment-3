using LiberaryBusiness.Item;
using LiberaryBusiness.Users.Member;
using LiberaryBusiness.Users.Staff;
using LiberaryBusiness.Users.Staff.Staff;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiberaryBusiness
{
	public static class Liberary
	{
		static int StudenLateFeeDiscount = 0;
		static int BookLoanDayes = 21;
		static int BookBaseLateFee = 1;
		static decimal BookRent = 10;
		static int DVDLoanDayes = 7;
		static decimal DVDRent = 40;
		static int DVDBaseLateFee = 2;
		static decimal MagazineRent = 5;
		static decimal MagazineBaseLateFee = 0.5m;
		static int MagazineLoanDayes = 3;


		static decimal HeadresponsibilityAllowance = 400;
		public static List<StudentMember> students = new()
		{
			new("Ahmed Mohamed", "01012345678", 3, 0m),
			new("Omar Ali", "01123456789", 3, 0m),
			new("Mahmoud Hassan", "01234567890", 3, 0m),
			new("Youssef Ibrahim", "01512345678", 3, 0m),
			new("Amr Khaled", "01098765432", 3, 0m),
			new("Karim Mostafa", "01198765432", 3, 0m),
			new("Ziad Samir", "01298765432", 3, 0m),
			new("Hossam Adel", "01598765432", 3, 0m),
			new("Ali Tarek", "01055556666", 3, 0m),
			new("Mostafa Ashraf", "01155556666", 3, 0m)
		};

		public static List<PremiumMember> premiumMembers = new()
		{
			new("Ahmed Samy", "01011112222", 10, 10m, 5),
			new("Mariam Ali", "01111112222", 10, 15m, 5),
			new("Nour Hassan", "01211112222", 10, 5m, 5),
			new("Salma Mohamed", "01511112222", 8, 10m, 5),
			new("Yara Khaled", "01022223333", 10, 20m, 5),
			new("Omar Tarek", "01122223333", 7, 10m, 5),
			new("Farida Adel", "01222223333", 9, 15m, 5),
			new("Seif Mostafa", "01522223333", 10, 5m, 5),
			new("Malak Ashraf", "01033334444", 8, 10m, 5),
			new("Adam Samir", "01133334444", 10, 15m, 5)
		};

		public static List<Librarian> librarians = new()
		{
			new("Hany Mohamed", "01044445551", new DateOnly(2020, 1, 10), 8000m, 0m),
			new("Mona Ali", "01144445551", new DateOnly(2021, 3, 15), 8500m, 0m),
			new("Tamer Hassan", "01244445551", new DateOnly(2019, 6, 20), 9000m, 0m),
			new("Rania Ibrahim", "01544445551", new DateOnly(2022, 2, 1), 7500m, 0m),
			new("Khaled Samy", "01044445552", new DateOnly(2018, 9, 5), 9500m, 0m),
			new("Dina Adel", "01144445552", new DateOnly(2023, 4, 12), 7000m, 0m),
			new("Ashraf Tarek", "01244445552", new DateOnly(2020, 8, 18), 8200m, 0m),
			new("Heba Mostafa", "01544445552", new DateOnly(2021, 11, 7), 7800m, 0m),
			new("Wael Samir", "01044445553", new DateOnly(2017, 5, 25), 10000m, 0m),
			new("Nadia Fathy", "01144445553", new DateOnly(2022, 7, 14), 7600m, 0m)
		};
		public static List<HeadLibrarian> headLibrarians = new()
		{
			new("Mostafa Kamel", "01077778888", new DateOnly(2015, 5, 10), 12000m, HeadresponsibilityAllowance),
			new("Fatma Zohdy", "01177778888", new DateOnly(2016, 8, 20), 11500m, HeadresponsibilityAllowance)
		};
		public static List<Shelver> shelfers = new()
		{
			new Shelver("Ibrahim Ahmed", "01088889991", new DateOnly(2022, 5, 12), 6000m, 200m, "Fiction Section"),
			new Shelver("Khaled Mahmoud", "01188889991", new DateOnly(2023, 1, 10), 5800m, 200m, "Science & Technology")
		};
		public static List<Book> books = new()
		{
			new("BK001", "Clean Code", BookLoanDayes,BookRent,BookBaseLateFee),
			new("BK002", "The Pragmatic Programmer", BookLoanDayes,BookRent,BookBaseLateFee),
			new("BK003", "Design Patterns", BookLoanDayes,BookRent,BookBaseLateFee),
			new("BK004", "C# in Depth", BookLoanDayes,BookRent,BookBaseLateFee),
			new("BK005", "Introduction to Algorithms", BookLoanDayes,BookRent,BookBaseLateFee)
		};
		public static List<Magazine> magazines = new()
		{
			new("MG001", "National Geographic - Jan 2026", MagazineLoanDayes,MagazineRent ,MagazineBaseLateFee),
			new("MG002", "Time Magazine - Tech Edition", MagazineLoanDayes, MagazineRent,MagazineBaseLateFee),
			new("MG003", "Scientific American - Space", MagazineLoanDayes,MagazineRent ,MagazineBaseLateFee),
			new("MG004", "The Economist - Global Markets", MagazineLoanDayes,MagazineRent ,MagazineBaseLateFee),
			new("MG005", "Forbes - Billionaires 2026", MagazineLoanDayes,MagazineRent ,MagazineBaseLateFee)
		};
		public static List<DVD> dvds = new()
		{
			new("DVD001", "C# Advanced Architecture Masterclass", DVDLoanDayes,DVDRent ,DVDBaseLateFee),
			new("DVD002", "Design Patterns in Practice", DVDLoanDayes,DVDRent ,DVDBaseLateFee),
			new("DVD003", "Clean Code Hands-On Workshop", DVDLoanDayes,DVDRent ,DVDBaseLateFee),
			new("DVD004", "Building Microservices with .NET", DVDLoanDayes,DVDRent ,DVDBaseLateFee),
			new("DVD005", "Data Structures & Algorithms Visualized", DVDLoanDayes,DVDRent ,DVDBaseLateFee)
		};
	}
}