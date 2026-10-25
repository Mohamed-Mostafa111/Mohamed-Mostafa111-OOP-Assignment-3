namespace LiberaryBusiness.Users.Staff
{
	public class Shelver : staff
	{
		public string Section { get; private set; }

		public Shelver(string FullName, string PhoneNumber,
						DateOnly hiredate, decimal monthlySalary,
						decimal responsibilityAllowance,string Section) : base(FullName, PhoneNumber, hiredate, monthlySalary, responsibilityAllowance)
		{
			
		}

		public void Reassign(string NewSection)
		{
			if(NewSection==Section)
			{
				throw new ArgumentException("this Section has already existed");
			}
			string oldSection = Section;
			Section = NewSection;
			Console.WriteLine($"Shelver {FullName} has been reasigned from {oldSection} to {NewSection}");
		}
	}
}
