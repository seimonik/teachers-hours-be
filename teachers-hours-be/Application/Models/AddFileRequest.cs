using TH.Dal.Enums;

namespace teachers_hours_be.Application.Models;

public class AddFileRequest
{
    public IFormFile File { get; set; } = null!;
    public DocumentTypes DocumentType { get; set; } = DocumentTypes.Ordinary;
}
