using BillTrack.Application.Services;
using BillTrack.Auth.Jwt;
using BillTrack.Auth.Utils;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Core.Interfaces.Utils;

namespace BillTrack.Api.Configurations;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IWebApiService, WebApiService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ISqsPublisher, SqsPublisher>();
        services.AddScoped<IS3FileService, S3FileService>();
        services.AddScoped<IJwtTokenCreator, FastEndpointsJwtTokenCreator>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}