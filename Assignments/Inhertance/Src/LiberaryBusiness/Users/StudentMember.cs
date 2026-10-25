namespace LiberaryBusiness.Users.Member
{
	public class StudentMember:Member
	{

	    public StudentMember(string FullName, string phoneNumber, int MaxLoans, decimal LateFeeDiscountPercent) 
			                : base(FullName, phoneNumber, MaxLoans, LateFeeDiscountPercent)
		{

		}
	}
}
