using TH.Dal.Enums;

namespace teachers_hours_be.Application.Models;

public class DocumentModel
{
	public Guid Id { get; set; }
	public string Name { get; set; } = String.Empty;
	public DateTime CreatedAt { get; set; }
	public DocumentTypes DocumentType { get; set; }
}
