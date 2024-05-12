using Amazon.S3.Transfer;
using MediatR;
using Microsoft.Extensions.Options;
using TH.Dal;
using TH.Dal.Entities;
using TH.Dal.Enums;
using TH.S3Client;

namespace teachers_hours_be.Application.Commands;

public static class AddDocument
{
    public record Command(IFormFile File, DocumentTypes DocumentType, int? EndRow = null) : IRequest<Document>;

	internal class Handler : IRequestHandler<Command, Document>
    {
        private readonly TeachersHoursDbContext _dbContext;
        private readonly ITransferUtility _transferUtility;
        private readonly S3Options _s3options;

        public Handler(TeachersHoursDbContext dbContext, ITransferUtility transferUtility, IOptions<S3Options> options)
        {
            _dbContext = dbContext;
            _transferUtility = transferUtility;
            _s3options = options.Value;
        }

        public async Task<Document> Handle(Command request, CancellationToken cancellationToken)
        {
            var filePath = request.DocumentType switch
            {
                DocumentTypes.Ordinary => $"all/{Guid.NewGuid()}",
                DocumentTypes.Request => $"request/{Guid.NewGuid()}",
                DocumentTypes.Calculation => $"calculation/{Guid.NewGuid()}",
                _ => throw new Exception()
            };

			var uploadRequest = new TransferUtilityUploadRequest
            {
                BucketName = _s3options.Bucket,
                Key = filePath,
                AutoCloseStream = true,
                InputStream = request.File.OpenReadStream()
            };
            await _transferUtility.UploadAsync(uploadRequest, cancellationToken);

            var document = new Document
            {
                Name = request.File.FileName,
                Url = filePath,
                CreatedAt = DateTime.UtcNow,
                DocumentType = request.DocumentType,
                EndRow = request.EndRow
            };
            _dbContext.Documents.Add(document);
            await _dbContext.SaveChangesAsync();

			return document;
        }
    }
}
