using Microsoft.AspNetCore.Http;
using TH.Services.Models;

namespace TH.Services.RenderServices;

public class RenderServiceContext
{
    public RenderServiceContext(GetTeachersHoursReportRequest request)
    {
        File = request.File;
        Faculty = request.Faculty;
        TimeNorms = request.TimeNorms;
        RowCount = request.RowCount;
        SpecialtiesRow = request.SpecialtiesRow;
        RateRow = request.RateRow;
    }

    public IFormFile File { get; set; } = null!;
    public string? Faculty { get; set; }
    public TimeNorms? TimeNorms { get; set; }
    public string? RowCount { get; set; }
    public int SpecialtiesRow { get; set; }
    public int RateRow { get; set; }
}

public interface IRenderService : IService<RenderServiceContext, Stream>
{
}
