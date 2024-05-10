using TH.Dal.Enums;

namespace teachers_hours_be.Application.Models.Lookups;

public class SpecializationModel
{
	public string Code { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public EducationLevelTypes EducationLevel { get; set; }
}
