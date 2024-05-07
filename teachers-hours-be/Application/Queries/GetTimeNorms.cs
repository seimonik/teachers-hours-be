using MediatR;
using Microsoft.EntityFrameworkCore;
using teachers_hours_be.Application.Models;
using teachers_hours_be.Extensions.ModelConversion;
using TH.Dal;

namespace teachers_hours_be.Application.Queries;

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
			var lookups = (await _dbContext.TimeNorms
				.AsNoTracking()
				.ToListAsync(cancellationToken)).Select(x => x.ToTimeNormModel());

			return lookups;
		}
	}
}
