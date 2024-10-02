using BillTrack.Persistence;
using BillTrack.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BillTrack.Worker.Configurations;

public static class DatabaseConfiguration
{
    public static IServiceCollection ConfigureDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(
            (options) =>
            {
                options.UseNpgsql(connectionString);
            });

        return services;
    }
}