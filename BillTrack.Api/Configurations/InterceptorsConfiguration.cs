using BillTrack.Persistence.Interceptors;

namespace BillTrack.Api.Configurations;

public static class InterceptorsConfiguration
{
    public static IServiceCollection ConfigureInterceptors(this IServiceCollection services)
    {
        services.AddSingleton<SoftDeleteInterceptor>();
        services.AddSingleton<UpdateAuditableEntityInterceptor>();
        
        return services;
    }
}