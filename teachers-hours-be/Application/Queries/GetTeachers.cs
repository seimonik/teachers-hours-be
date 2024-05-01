using MediatR;
using Microsoft.EntityFrameworkCore;
using teachers_hours_be.Application.Models;
using teachers_hours_be.Extensions.ModelConversion;
using TH.Dal;

namespace teachers_hours_be.Application.Queries;

public static class GetTeachers
{
	public record Query() : IRequest<IEnumerable<GetTeacherModel>>;

	internal class Handler : IRequestHandler<Query, IEnumerable<GetTeacherModel>>
	{
		private readonly TeachersHoursDbContext _dbContext;

		public Handler(TeachersHoursDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<GetTeacherModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			var query = await _dbContext.Teachers
				.AsNoTracking()
				.ToListAsync();

			return query.Select(x => x.ToGetTeacherModel());
		}
	}
}
