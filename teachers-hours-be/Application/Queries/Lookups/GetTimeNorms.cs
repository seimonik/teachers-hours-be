using MediatR;
using Microsoft.EntityFrameworkCore;
using teachers_hours_be.Application.Models.Lookups;
using teachers_hours_be.Extensions.ModelConversion;
using TH.Dal;

namespace teachers_hours_be.Application.Queries.Lookups;

public static class GetTimeNorms
{
	public record Query() : IRequest<IEnumerable<TimeNormModel>>;

	internal class Handler : IRequestHandler<Query, IEnumerable<TimeNormModel>>
	{
		private readonly TeachersHoursDbContext _dbContext;

		public Handler(TeachersHoursDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<TimeNormModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			var specializations = await _dbContext.TimeNorms
				.AsNoTracking()
				.ToListAsync();

			return specializations.Select(x => x.ToTimeNormModel());
		}
	}
}
