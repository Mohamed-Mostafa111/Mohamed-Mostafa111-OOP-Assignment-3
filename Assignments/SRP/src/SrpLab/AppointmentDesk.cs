namespace SrpLab;

/// <summary>
/// Appointment desk: opening-hours rules, slot search, and ICS calendar blob generation.
/// </summary>
public sealed class AppointmentDesk
{
    private readonly HashSet<DateTimeOffset> _booked = new();
    public TimeOnly Open { get; }
    public TimeOnly Close { get; }
    public int SlotMinutes { get; }

	private readonly AppointmentReminderService AppointmentService;
	private readonly BusinessHoursAndSlotSearch hoursAndSlotSearch;

	public AppointmentDesk(TimeOnly open, TimeOnly close, int slotMinutes)
	{
		Open = open;
		Close = close;
		SlotMinutes = slotMinutes;
		this.hoursAndSlotSearch = new BusinessHoursAndSlotSearch(_booked,Open,Close,SlotMinutes);
		AppointmentService = new AppointmentReminderService(slotMinutes,hoursAndSlotSearch,_booked);
	}
	public DateTimeOffset? FindNextSlot(DateTimeOffset from, int searchHours)
	{
		return AppointmentService.FindNextSlot(from, searchHours);
	}

	public bool TryBook(DateTimeOffset slot)
	{
		return hoursAndSlotSearch.TryBook(slot);
	}

	public string SmsReminder(DateTimeOffset slot, string clinicPhone)
	{
		return AppointmentService.SmsReminder(slot, clinicPhone);
	}

}

public sealed class BusinessHoursAndSlotSearch
{
    private readonly HashSet<DateTimeOffset> _booked;
    private readonly TimeOnly Open;
    private readonly TimeOnly Close;
    private readonly int SlotMinutes;

	public BusinessHoursAndSlotSearch(HashSet<DateTimeOffset> booked, TimeOnly open, TimeOnly close, int slotMinutes)
	{
		_booked = booked;
		Open = open;
		Close = close;
		SlotMinutes = slotMinutes;
	}

	public bool IsWithinBusinessHours(DateTimeOffset when)
	{
		// Clinic calendar policy — HR/ops — not the same as ICS serialization.
		if (when.DayOfWeek is DayOfWeek.Friday or DayOfWeek.Saturday) return false;
		var t = TimeOnly.FromDateTime(when.DateTime);
		return t >= Open && t.AddMinutes(SlotMinutes) <= Close;
	}



	public bool TryBook(DateTimeOffset slot)
	{
		if (!IsWithinBusinessHours(slot) || _booked.Contains(slot)) return false;
		_booked.Add(slot);
		return true;
    }
}
public sealed class AppointmentReminderService
{
	private readonly int SlotMinutes;
	private readonly BusinessHoursAndSlotSearch slotSearch;
	private readonly IReadOnlySet<DateTimeOffset> _booked;
	public AppointmentReminderService(int slotMinutes, BusinessHoursAndSlotSearch slotSearch, IReadOnlySet<DateTimeOffset> booked)
	{
		SlotMinutes = slotMinutes;
		this.slotSearch = slotSearch;
		_booked = booked;
	}

	public string SmsReminder(DateTimeOffset slot, string clinicPhone)
	{
		// Messaging channel copy — fourth concern hiding in the "scheduler".
		return $"Reminder: appointment {slot:MMM dd HH:mm}. Call {clinicPhone} to reschedule.";
	}

	private DateTimeOffset Align(DateTimeOffset from)
	{
		var minutes = from.Minute - (from.Minute % SlotMinutes);
		return new DateTimeOffset(from.Year, from.Month, from.Day, from.Hour, minutes, 0, from.Offset);
	}

	public DateTimeOffset? FindNextSlot(DateTimeOffset from, int searchHours)
	{
		var cursor = Align(from);
		var end = from.AddHours(searchHours);
		while (cursor < end)
		{
			if (slotSearch.IsWithinBusinessHours(cursor) && !_booked.Contains(cursor))
				return cursor;
			cursor = cursor.AddMinutes(SlotMinutes);
		}
		return null;
	}
}

public sealed class CalendarIcsExporter
{
	private readonly int SlotMinutes;

	public CalendarIcsExporter(int slotMinutes)
	{
		SlotMinutes = slotMinutes;
	}

	public string ToIcs(DateTimeOffset slot, string patientName, string clinician)
	{
		// Calendar interoperability format changes with clients — not opening hours.
		var uid = Guid.NewGuid();
		var end = slot.AddMinutes(SlotMinutes);
		return "BEGIN:VCALENDAR\nVERSION:2.0\nBEGIN:VEVENT\n" +
			   $"UID:{uid}\nDTSTART:{slot:yyyyMMdd'T'HHmmss'Z'}\nDTEND:{end:yyyyMMdd'T'HHmmss'Z'}\n" +
			   $"SUMMARY:Visit {patientName} / {clinician}\nEND:VEVENT\nEND:VCALENDAR\n";
	}
}