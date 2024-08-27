using BillTrack.Application.Services;
using BillTrack.Core.Interfaces.Services;

namespace BillTrack.Api.Configurations;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IWebApiService, WebApiService>();

        return services;
    }
}