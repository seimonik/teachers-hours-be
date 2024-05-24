using Amazon.S3.Model;
using Amazon.S3.Transfer;
using MediatR;
using Microsoft.Extensions.Options;
using TH.Dal;
using TH.S3Client;

namespace teachers_hours_be.Application.Commands;

public static class DeleteDocument
{
	public record Command(Guid DocumentId) : IRequest<Unit>;

	internal class Handler : IRequestHandler<Command, Unit>
	{
		private readonly TeachersHoursDbContext _dbContext;
		private readonly ITransferUtility _transferUtility;
		private readonly S3Options _s3options;

		public Handler(TeachersHoursDbContext dbContext,
					   ITransferUtility transferUtility,
					   IOptions<S3Options> options)
		{
			_dbContext = dbContext;
			_transferUtility = transferUtility;
			_s3options = options.Value;
		}

		public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
		{
			var documentDb = _dbContext.Documents
				.Where(d => d.Id == request.DocumentId)
				.Single();

			await _transferUtility.S3Client.DeleteObjectAsync(
			new DeleteObjectRequest
				{
					BucketName = _s3options.Bucket,
					Key = documentDb.Url
				}, cancellationToken);

			_dbContext.Documents.Remove(documentDb);
			await _dbContext.SaveChangesAsync();

			return Unit.Value;
		}
	}
}
