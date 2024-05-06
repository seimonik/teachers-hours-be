namespace TH.Dal.Entities;

public class TimeNorm
{
	public Guid Id { get; set; }
	public string Code { get; set; } = String.Empty;
	public string Name { get; set; } = String.Empty;
	public double Norm { get; set; }
}
