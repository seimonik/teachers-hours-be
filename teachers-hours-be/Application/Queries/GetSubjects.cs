using Amazon.S3.Model;
using Amazon.S3.Transfer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TH.Dal;
using TH.S3Client;
using TH.Services.Models;
using TH.Services.ParsingSevice;

namespace teachers_hours_be.Application.Queries;

public static class GetSubjects
{
	public record Query(Guid DocumentId) : IRequest<IEnumerable<SubjectModel>>;

	internal class Handler : IRequestHandler<Query, IEnumerable<SubjectModel>>
	{
		private readonly IParsingService _parsingService;
		private readonly TeachersHoursDbContext _dbContext;
		private readonly ITransferUtility _transferUtility;
		private readonly S3Options _s3options;

		public Handler(IParsingService parsingService, 
					   TeachersHoursDbContext dbContext, 
					   ITransferUtility transferUtility, 
					   IOptions<S3Options> options)
		{
			_parsingService = parsingService;
			_dbContext = dbContext;
			_transferUtility = transferUtility;
			_s3options = options.Value;
		}

		public async Task<IEnumerable<SubjectModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			var document = _dbContext.Documents
				.AsNoTracking()
				.Where(d => d.Id == request.DocumentId)
				.SingleOrDefault();

			var requestS3 = new GetObjectRequest
			{
				BucketName = _s3options.Bucket,
				Key = document!.Url,
			};
			using GetObjectResponse response = await _transferUtility.S3Client.GetObjectAsync(requestS3);
			return await _parsingService.ExecuteAsync(
				new ParsingServiceContext(response.ResponseStream), cancellationToken);
		}
	}
}
