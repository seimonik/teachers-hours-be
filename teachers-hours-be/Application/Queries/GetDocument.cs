using MediatR;
using teachers_hours_be.Application.Models;
using TH.Dal;
using Microsoft.EntityFrameworkCore;
using teachers_hours_be.Extensions.ModelConversion;

namespace teachers_hours_be.Application.Queries;

public static class GetDocument
{
	public record Query(Guid DocumentId) : IRequest<DocumentModel>;

	internal class Handler : IRequestHandler<Query, DocumentModel>
	{
		private readonly TeachersHoursDbContext _dbContext;

		public Handler(TeachersHoursDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<DocumentModel> Handle(Query request, CancellationToken cancellationToken)
		{
			var query = await _dbContext.Documents
				.Include(d => d.ChildDocuments)
				.Where(d => d.Id == request.DocumentId)
				.AsNoTracking()
				.SingleAsync(cancellationToken);

			return query.ToDocumentModel();
		}
	}
}
