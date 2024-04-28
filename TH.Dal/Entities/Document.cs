namespace TH.Dal.Entities;

public class Document
{
	public Guid Id { get; set; }
	public string Name { get; set; } = String.Empty;
	public string Url { get; set; } = String.Empty;
	public DateTime CreatedAt { get; set; }
}
