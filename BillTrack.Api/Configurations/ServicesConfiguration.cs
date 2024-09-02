using BillTrack.Application.Services;
using BillTrack.Auth.Jwt;
using BillTrack.Core.Interfaces.Services;

namespace BillTrack.Api.Configurations;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IWebApiService, WebApiService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}