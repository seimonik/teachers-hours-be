namespace TH.Services.Models;

public class TeacherSubject
{
	public string Name { get; set; } = null!;
	public string? Specialization { get; set; }
	public int? Semester { get; set; }
	public int? Budget { get; set; }
	public int? Commercial { get; set; }
	public string? Groups { get; set; }
	public string GroupForm { get; set; } = null!;
	public int? TotalHours { get; set; }
	public int? Lectures { get; set; }
	public int? Seminars { get; set; }
	public int? Laboratory { get; set; }
	public int? SelfStudy { get; set; }
	public int? LoadPerWeek { get; set; }
	public string? ReportingForm { get; set; }
	public string? Remark { get; set; }
	public string TeacherName { get; set; } = string.Empty!;
	public bool practice { get; set; }
}
