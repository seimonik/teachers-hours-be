using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TH.S3Client;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddS3(this IServiceCollection services, IConfiguration configuration)
    {
        var optinos = configuration.Get<S3Options>();
        services.Configure<S3Options>(options => configuration.Bind(options));
        services.AddScoped<IAmazonS3>(_ => new AmazonS3Client(
            awsAccessKeyId: optinos!.UserName,
            awsSecretAccessKey: optinos.Password,
            clientConfig: optinos.ToAmazonS3Config()));
        services.AddScoped<ITransferUtility, TransferUtility>();
        return services;
    }
}
