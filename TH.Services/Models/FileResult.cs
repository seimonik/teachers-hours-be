namespace TH.Services.Models;

public class FileResult
{
    public MemoryStream File { get; set; } = null!;
    public string MimeType { get; set; } = null!;
    public string FileName { get; set; } = null!;
}
