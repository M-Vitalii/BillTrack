using BillTrack.Core.Models;

namespace BillTrack.Api.Configurations;

public static class AppSettingsConfiguration
{
    public static IServiceCollection ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AwsSettings>(configuration.GetSection(AwsSettings.SectionName));

        return services;
    }
}