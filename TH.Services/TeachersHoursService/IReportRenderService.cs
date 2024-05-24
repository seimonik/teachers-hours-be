using TH.Services.Models;

namespace TH.Services.TeachersHoursService;

public class ReportRenderServiceContext
{
	public ReportRenderServiceContext(
		Stream file,
		string faculty,
		TimeNorms timeNorms,
		SpecializationsModel specializations,
		IEnumerable<TeacherRateModel> teacherRates)
	{
		File = file;
		Faculty = faculty;
		TimeNorms = timeNorms;
		Specializations = specializations;
		TeacherRates = teacherRates;
	}

	public Stream File { get; set; } = null!;
	public string Faculty { get; set; } = string.Empty;
	public TimeNorms TimeNorms { get; set; }
	public SpecializationsModel Specializations { get; set; } = null!;
	public IEnumerable<TeacherRateModel> TeacherRates { get; set; }
}

public interface IReportRenderService : IService<ReportRenderServiceContext, Stream>
{
}
