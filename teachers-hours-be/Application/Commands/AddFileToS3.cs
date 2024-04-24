using Amazon.S3.Transfer;
using MediatR;
using Microsoft.Extensions.Options;
using TH.S3Client;

namespace teachers_hours_be.Application.Commands;

public class AddFileToS3
{
    public record Query(IFormFile File) : IRequest<Unit>;

    internal class Handler : IRequestHandler<Query, Unit>
    {
        private readonly ITransferUtility _transferUtility;
        private readonly S3Options _s3options;

        public Handler(ITransferUtility transferUtility, IOptions<S3Options> options)
        {
            _transferUtility = transferUtility;
            _s3options = options.Value;
        }

        public async Task<Unit> Handle(Query request, CancellationToken cancellationToken)
        {
            var uploadRequest = new TransferUtilityUploadRequest
            {
                BucketName = _s3options.Bucket,
                Key = $"test", // TODO: Продумать путь хранения файлов
                AutoCloseStream = true,
                InputStream = request.File.OpenReadStream()
            };
            await _transferUtility.UploadAsync(uploadRequest, cancellationToken);

            return Unit.Value;
        }
    }
}
