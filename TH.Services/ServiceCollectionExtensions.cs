﻿using Microsoft.Extensions.DependencyInjection;
using TH.Services.GenerateDocxService;
using TH.Services.ParsingSevice;
using TH.Services.RenderServices;

namespace TH.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExcelGeneratorServices(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddScoped<IRenderService, TeachersHoursReportRenderService>();
        services.AddScoped<IParsingService, GetSubjects>();
        services.AddScoped<IAddTeachersService, AddTeachersService>();
        services.AddScoped<IRenderCourseworkJournalService, RenderCourseworkJournalService>();

        return services;
    }
}
