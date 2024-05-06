namespace teachers_hours_be.Application.Models;

public class FileDownloadResult
{
	public byte[] FileByteArray { get; set; } = null!;
	public string MimeType { get; set; } = string.Empty;
	public string FileName { get; set; } = string.Empty;
}
