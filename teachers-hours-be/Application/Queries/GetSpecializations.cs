using MediatR;
using Microsoft.EntityFrameworkCore;
using TH.Dal;
using TH.Dal.Enums;
using TH.Services.Models;

namespace teachers_hours_be.Application.Queries;

public static class GetSpecializations
{
	public record Query() : IRequest<SpecializationsModel>;

	internal class Handler : IRequestHandler<Query, SpecializationsModel>
	{
		private readonly TeachersHoursDbContext _dbContext;

		public Handler(TeachersHoursDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<SpecializationsModel> Handle(Query request, CancellationToken cancellationToken)
		{
			var specializations = await _dbContext.Specializations
				.AsNoTracking()
				.ToListAsync();

			return new SpecializationsModel
			{
				Bachelor = specializations
				.Where(x => x.EducationLevel == EducationLevelTypes.Bachelor)
				.ToDictionary(x => x.Code, x => x.Name),
				Specialty = specializations
				.Where(x => x.EducationLevel == EducationLevelTypes.Specialty)
				.ToDictionary(x => x.Code, x => x.Name),
				Magistracy = specializations
				.Where(x => x.EducationLevel == EducationLevelTypes.Magistracy)
				.ToDictionary(x => x.Code, x => x.Name),
				Postgraduate = specializations
				.Where(x => x.EducationLevel == EducationLevelTypes.Postgraduate)
				.ToDictionary(x => x.Code, x => x.Name)
			};
		}
	}
}
