namespace TH.Dal.Entities;

public class Teacher
{
	public Guid Id { get; set; }
	public string FullName { get; set; } = String.Empty;
	public string Name { get; set; } = String.Empty;
	public string Surname { get; set; } = String.Empty;
	public string Patronymic { get; set; } = String.Empty;
	public int Rate { get; set; }
}
