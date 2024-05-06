using Amazon.S3.Model;
using Amazon.S3.Transfer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using teachers_hours_be.Application.Models;
using teachers_hours_be.Constants;
using TH.Dal;
using TH.S3Client;

namespace teachers_hours_be.Application.Queries;

public static class GetDocumentFile
{
	public record Query(Guid DocumentId) : IRequest<FileDownloadResult>;

	internal class Handler : IRequestHandler<Query, FileDownloadResult>
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

		public async Task<FileDownloadResult> Handle(Query request, CancellationToken cancellationToken)
		{
			var document = _dbContext.Documents
				.Where(d => d.Id == request.DocumentId)
				.AsNoTracking()
				.SingleOrDefault();

			var requestS3 = new GetObjectRequest
			{
				BucketName = _s3options.Bucket,
				Key = document!.Url,
			};
			using GetObjectResponse response = await _transferUtility.S3Client.GetObjectAsync(requestS3);
			using MemoryStream ms = new MemoryStream();
			response.ResponseStream.CopyTo(ms);

			var mimeType = Path.GetExtension(document!.Name) switch
			{
				".docx" => MimeTypes.Docx,
				".xlsx" => MimeTypes.Xlsx,
				_ => throw new ArgumentException()
			};

			return new FileDownloadResult
			{
				FileByteArray = ms.ToArray(),
				MimeType = mimeType,
				FileName = document!.Name
			};
		}
	}
}
