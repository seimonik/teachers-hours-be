using Microsoft.Extensions.DependencyInjection;
using TH.Services.RenderServices;

namespace TH.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExcelGeneratorServices(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        // TODO: Добавить сервис для генерации excel отчета
        services.AddScoped<IRenderService, TeachersHoursReportRenderService>();

        return services;
    }
}
