namespace TH.Services.Models;

public class TeacherGenerateModel
{
	public string FullName { get; set; } = "";
	public int FirstSemesterBudgetRow { get; set; }
	public int SecondSemesterBudgetRow { get; set; }
	public int FirstSemesterCommercialRow { get; set; }
	public int SecondSemesterCommercialRow { get; set; }
}
