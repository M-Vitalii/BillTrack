global using FluentValidation;
using BillTrack.Api.Configurations;
using FastEndpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();
builder.Logging.AddConsole();

builder.Services
    .ConfigureInterceptors()
    .ConfigureDatabase(builder.Configuration)
    .ConfigureRepositories()
    .ConfigureServices()
    .ConfigureMappers();

var app = builder.Build();

app.UseExceptionHandler(_ => { });
app.UseFastEndpoints(c =>
{
    c.Endpoints.RoutePrefix = "api";
});

app.Run();
