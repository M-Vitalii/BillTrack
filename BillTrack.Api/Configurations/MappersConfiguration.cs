
using BillTrack.Core.AutoMapperProfiles;

namespace BillTrack.Api.Configurations;

public static class MappersConfiguration
{
    public static IServiceCollection ConfigureMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DepartmentProfile).Assembly);

        return services;
    }
}