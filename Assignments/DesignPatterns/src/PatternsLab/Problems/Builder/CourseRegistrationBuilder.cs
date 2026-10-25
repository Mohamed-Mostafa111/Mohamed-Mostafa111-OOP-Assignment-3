using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternsLab.Problems.Builder
{
	public  class CourseRegistrationBuilder
	{
		private string StudentEmail = "";
		private string CourseCode = "";
		private string AccessMode = "";
		private string? GroupCode = "";
		private string? DiscountCode = "";
		private bool SendWhatsApp;
		private bool SendEmailWelcome;
		private string? MentorNote;
		private DateOnly? PreferredStart;
		public CourseRegistrationBuilder SetStudentAndCourseInfo(string Email,string CourseCode)
		{
			StudentEmail = Email;
			this.CourseCode = CourseCode;
			return this;
		}
		public CourseRegistrationBuilder SetEnrollmentOptions(string AccessMode,string GroupCode,DateOnly? preferredStart)
		{
			this.AccessMode = AccessMode;
			this.GroupCode = GroupCode;
			PreferredStart = preferredStart;
			return this;
		}
		public CourseRegistrationBuilder SetDiscount(string DiscountCode)
		{
			this.DiscountCode = DiscountCode;
			return this;
		}
		public CourseRegistrationBuilder SetNotificationPreferences(bool SendEmail,bool SendWhatsapp)
		{
			SendEmailWelcome = SendEmail;
			SendWhatsapp = SendWhatsApp;
			return this;
		}
		public CourseRegistrationBuilder SetMentorNote(string MentorNote)
		{
			this.MentorNote = MentorNote;
			return this;
		}

		public CourseRegistration Build()
		{
			return new CourseRegistration(
				StudentEmail, 
				CourseCode, 
				AccessMode, 
				GroupCode, 
				DiscountCode, 
				SendWhatsApp, 
				SendEmailWelcome, 
				MentorNote, 
				PreferredStart);
		}
	}
}
