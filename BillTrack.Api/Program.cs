global using FluentValidation;
using BillTrack.Api.Configurations;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthenticationJwtBearer(s => s.SigningKey = builder.Configuration["JwtSecretKey"])
    .AddAuthorization()
    .AddFastEndpoints()
    .SwaggerDocument();

builder.Logging.AddConsole();

builder.Services
    .ConfigureInterceptors()
    .ConfigureDatabase(builder.Configuration)
    .ConfigureRepositories()
    .ConfigureServices()
    .ConfigureMappers();

var app = builder.Build();

app.Seed();

app.UseExceptionHandler(_ => { });

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(c =>
{
    c.Endpoints.RoutePrefix = "api";
});

app.UseSwaggerGen();

app.Run();
