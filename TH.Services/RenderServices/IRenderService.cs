using Microsoft.AspNetCore.Http;
using TH.Services.Models;

namespace TH.Services.RenderServices;

public class RenderServiceContext
{
    public RenderServiceContext(
		IFormFile file,
		string faculty,
		TimeNorms timeNorms,
        int rowCount,
		SpecializationsModel specializations,
        IEnumerable<TeacherRateModel> teacherRates)
    {
        File = file;
        Faculty = faculty;
        TimeNorms = timeNorms;
        RowCount = rowCount;
		Specializations = specializations;
		TeacherRates = teacherRates;
    }

    public IFormFile File { get; set; } = null!;
    public string Faculty { get; set; } = string.Empty;
    public TimeNorms TimeNorms { get; set; }
    public int RowCount { get; set; }
    public SpecializationsModel Specializations { get; set; } = null!;
    public IEnumerable<TeacherRateModel> TeacherRates { get; set; }
}

public interface IRenderService : IService<RenderServiceContext, Stream>
{
}
