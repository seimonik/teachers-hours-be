namespace TH.Services.Models;

public class SpecializationsModel
{
	/// <summary>
	/// Бакалавриат
	/// </summary>
	public Dictionary<string, string> Bachelor { get; set; } = null!;

	/// <summary>
	/// Специалитет
	/// </summary>
	public Dictionary<string, string> Specialty { get; set; } = null!;

	/// <summary>
	/// Магистратура
	/// </summary>
	public Dictionary<string, string> Magistracy { get; set; } = null!;

	/// <summary>
	/// Аспиранутра
	/// </summary>
	public Dictionary<string, string> Postgraduate { get; set; } = null!;
}
