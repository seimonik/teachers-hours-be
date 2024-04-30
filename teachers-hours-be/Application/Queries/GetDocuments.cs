using MediatR;
using Microsoft.EntityFrameworkCore;
using teachers_hours_be.Application.Models;
using teachers_hours_be.Extensions.ModelConversion;
using TH.Dal;

namespace teachers_hours_be.Application.Queries;

public static class GetDocuments
{
	public record Query() : IRequest<IEnumerable<DocumentModel>>;

	internal class Handler : IRequestHandler<Query, IEnumerable<DocumentModel>>
	{
		private readonly TeachersHoursDbContext _dbContext;

		public Handler(TeachersHoursDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<DocumentModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			var query = await _dbContext.Documents
				.AsNoTracking()
				.ToListAsync();

			return query.Select(x => x.ToDocumentModel());
		}
	}
}
