using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TH.Services.Models;

public class GetTeachersHoursReportRequest
{
    [Required]
    public IFormFile File { get; set; } = null!;

    /// <summary>
    /// Факультет.
    /// </summary>
    public string? Faculty { get; set; }

    /// <summary>
    /// Нормы времени (обычно определяются по официальному документу на учебный год).
    /// </summary>
    public TimeNorms? TimeNorms { get; set; }
    public int RowCount { get; set; }
    public int SpecialtiesRow { get; set; }
    public int RateRow { get; set; }
}
