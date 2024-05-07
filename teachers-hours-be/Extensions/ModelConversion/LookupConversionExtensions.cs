using teachers_hours_be.Application.Models;
using TH.Dal.Entities;

namespace teachers_hours_be.Extensions.ModelConversion;

public static class LookupConversionExtensions
{
	public static TimeNormModel ToTimeNormModel(this TimeNorm norm) =>
		new TimeNormModel
		{
			Name = norm.Name,
			Code = norm.Code,
			Norm = norm.Norm
		};
}
