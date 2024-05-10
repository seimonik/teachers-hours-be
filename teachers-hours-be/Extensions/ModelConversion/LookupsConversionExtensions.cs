using teachers_hours_be.Application.Models.Lookups;
using TH.Dal.Entities;

namespace teachers_hours_be.Extensions.ModelConversion;

public static class LookupsConversionExtensions
{
	public static Specialization ToSpecialization(this SpecializationModel specialization) =>
		new Specialization
		{
			Name = specialization.Name,
			Code = specialization.Code,
			EducationLevel = specialization.EducationLevel
		};

	public static SpecializationModel ToSpecializationModel(this Specialization specialization) =>
		new SpecializationModel
		{
			Name = specialization.Name,
			Code = specialization.Code,
			EducationLevel = specialization.EducationLevel
		};

	public static TimeNormModel ToTimeNormModel(this TimeNorm norm) =>
		new TimeNormModel
		{
			Name = norm.Name,
			Code = norm.Code,
			Norm = norm.Norm
		};

	public static TimeNorm ToTimeNorm(this TimeNormModel norm) =>
		new TimeNorm
		{
			Name = norm.Name,
			Code = norm.Code,
			Norm = norm.Norm
		};
}
