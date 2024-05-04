using Amazon.S3.Model;
using Amazon.S3.Transfer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TH.Dal;
using TH.S3Client;
using TH.Services.RenderServices;

namespace teachers_hours_be.Application.Commands;

public static class AddTeachersToExcelDocument
{
	public record Command(Guid DocumentId,
						  IEnumerable<string> TeachersFullNames) : IRequest<byte[]>;

	internal class Handler : IRequestHandler<Command, byte[]>
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

		public async Task<byte[]> Handle(Command request, CancellationToken cancellationToken)
		{
			var document = _dbContext.Documents
				.Where(d=>d.Id == request.DocumentId)
				.AsNoTracking()
				.SingleOrDefault();

			var requestS3 = new GetObjectRequest
			{
				BucketName = _s3options.Bucket,
				Key = document!.Url,
			};
			using GetObjectResponse response = await _transferUtility.S3Client.GetObjectAsync(requestS3);

			return await _addTeachersService.ExecuteAsync(new AddTeachersServiceContext(
				response.ResponseStream, request.TeachersFullNames), cancellationToken);
		}
	}
}
