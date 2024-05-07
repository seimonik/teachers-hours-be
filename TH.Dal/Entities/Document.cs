using TH.Dal.Enums;

namespace TH.Dal.Entities;

public class Document
{
	public Guid Id { get; set; }
	public string Name { get; set; } = String.Empty;
	public string Url { get; set; } = String.Empty;
	public DateTime CreatedAt { get; set; }
	public DocumentTypes DocumentType { get; set; }
	public Guid? ParentDocumentId { get; set; }
	public int? EndRow { get; set; }

	public Document? ParentDocument { get; set; }
	public ICollection<Document> ChildDocuments { get; set; } = null!;
}
