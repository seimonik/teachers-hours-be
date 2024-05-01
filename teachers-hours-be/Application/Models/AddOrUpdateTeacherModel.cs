namespace teachers_hours_be.Application.Models;

public class AddOrUpdateTeacherModel
{
	public string Name { get; set; } = String.Empty;
	public string Surname { get; set; } = String.Empty;
	public string Patronymic { get; set; } = String.Empty;
	public int Rate { get; set; }
}
