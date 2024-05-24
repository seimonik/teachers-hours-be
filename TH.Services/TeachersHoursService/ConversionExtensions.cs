using TH.Services.Constants;
using TH.Services.Models;

namespace TH.Services.TeachersHoursService;

public static class ConversionExtensions
{
	public static IEnumerable<TeacherSubject> ToTeacherSubject(this Subject subject)
	{
		var teacherSubjects = new List<TeacherSubject>();
		foreach (var teacher in subject.Teacher)
		{
			teacherSubjects.Add(new TeacherSubject
			{
				Name = subject.Name,
				Specialization = subject.Specialization,
				Semester = subject.Semester,
				Budget = (SubjectNames.SubjectsDividedIntoTeachers.Contains(subject.Name) && subject.Budget.HasValue)
					? teacher.StudentsCount : subject.Budget,
				Commercial = (SubjectNames.SubjectsDividedIntoTeachers.Contains(subject.Name) && subject.Commercial.HasValue)
					? teacher.StudentsCount : subject.Commercial,
				Groups = subject.Groups,
				GroupForm = subject.GroupForm,
				TotalHours = subject.TotalHours,
				Lectures = subject.Lectures,
				Seminars = subject.Seminars,
				Laboratory = subject.Laboratory,
				SelfStudy = subject.SelfStudy,
				LoadPerWeek = subject.LoadPerWeek,
				ReportingForm = subject.ReportingForm,
				Remark = subject.Remark,
				practice = subject.practice,
				TeacherName = teacher.TeacherName
			});
		}
		return teacherSubjects;
	}
}
