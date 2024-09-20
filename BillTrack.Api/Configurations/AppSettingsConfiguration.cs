namespace BillTrack.Api.Configurations;

public static class AppSettingsConfiguration
{
    public static IServiceCollection ConfigureSettingsModels(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AwsSettings>(configuration.GetSection(AwsSettings.SectionName));

        return services;
    }
}