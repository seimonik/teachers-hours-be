using Amazon.S3.Model;
using Amazon.S3.Transfer;
using MediatR;
using Microsoft.Extensions.Options;
using teachers_hours_be.Application.Models;
using teachers_hours_be.Extensions.ModelConversion;
using TH.Dal;
using TH.S3Client;
using TH.Services.RenderServices;

namespace teachers_hours_be.Application.Commands;

public static class AddTeachersToExcelDocument
{
	public record Command(Guid DocumentId,
						  IEnumerable<string> TeachersFullNames) : IRequest<DocumentModel>;

	internal class Handler : IRequestHandler<Command, DocumentModel>
	{
		private readonly TeachersHoursDbContext _dbContext;
		private readonly IAddTeachersService _addTeachersService;
		private readonly ITransferUtility _transferUtility;
		private readonly S3Options _s3options;

		public Handler(TeachersHoursDbContext dbContext, 
					   IAddTeachersService addTeachersService, 
					   ITransferUtility transferUtility,
					   IOptions<S3Options> options)
		{
			_dbContext = dbContext;
			_addTeachersService = addTeachersService;
			_transferUtility = transferUtility;
			_s3options = options.Value;
		}

		public async Task<DocumentModel> Handle(Command request, CancellationToken cancellationToken)
		{
			var documentDb = _dbContext.Documents
				.Where(d=>d.Id == request.DocumentId)
				.SingleOrDefault();

			var requestS3 = new GetObjectRequest
			{
				BucketName = _s3options.Bucket,
				Key = documentDb!.Url,
			};
			using GetObjectResponse response = await _transferUtility.S3Client.GetObjectAsync(requestS3);

			var updatedFile = await _addTeachersService.ExecuteAsync(new AddTeachersServiceContext(
				response.ResponseStream, request.TeachersFullNames), cancellationToken);

			var filePath = $"request/{Guid.NewGuid()}";
			await _transferUtility.UploadAsync(
				new TransferUtilityUploadRequest
				{
					BucketName = _s3options.Bucket,
					Key = filePath,
					AutoCloseStream = true,
					InputStream = new MemoryStream(updatedFile)
				}, cancellationToken);

			await _transferUtility.S3Client.DeleteObjectAsync(
				new DeleteObjectRequest
				{
					BucketName = _s3options.Bucket,
					Key = documentDb.Url
				}, cancellationToken);

			documentDb.Url = filePath;
			await _dbContext.SaveChangesAsync();

			return documentDb.ToDocumentModel();
		}
	}
}
