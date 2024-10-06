using BillTrack.Core.Interfaces.Repositories;
using BillTrack.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BillTrack.Worker.Configurations;

public static class RepositoriesConfiguration
{
    public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        
        return services;
    }
}