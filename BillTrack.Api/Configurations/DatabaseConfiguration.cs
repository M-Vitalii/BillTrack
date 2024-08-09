using BillTrack.Persistence;
using BillTrack.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace BillTrack.Api.Configurations;

public static class DatabaseConfiguration
{
    public static IServiceCollection ConfigureDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(
            (serviceProvider, options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                    .AddInterceptors(
                        serviceProvider.GetService<SoftDeleteInterceptor>()!,
                        serviceProvider.GetService<UpdateAuditableEntityInterceptor>()!);
            });

        return services;
    }
}