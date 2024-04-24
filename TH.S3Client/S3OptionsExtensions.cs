using Amazon.S3;

namespace TH.S3Client;

public static class S3OptionsExtensions
{
    public static AmazonS3Config ToAmazonS3Config(this S3Options options) =>
        new()
        {
            ServiceURL = options.Host,
            ForcePathStyle = true
        };
}
