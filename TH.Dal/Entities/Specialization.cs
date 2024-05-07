using TH.Dal.Enums;

namespace TH.Dal.Entities;

/// <summary>
/// Направления/специальности
/// </summary>
public class Specialization
{
	public string Code { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public EducationLevelTypes EducationLevel { get; set; }
}
