using BillTrack.Core.Interfaces.Repositories;
using BillTrack.Persistence.Repositories;

namespace BillTrack.Api.Configurations;

public static class RepositoriesConfiguration
{
    public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        
        return services;
    }
}