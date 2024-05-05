namespace TH.Dal.Entities;

public class Request
{
	public Guid Id { get; set; }
	public Guid DocumentId { get; set; }
	public Guid ParentDocumentId { get; set; }
	public int EndRow { get; set; }
}
