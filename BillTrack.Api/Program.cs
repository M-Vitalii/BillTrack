using BillTrack.Api.Configurations;
using FastEndpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();

builder.Services
    .ConfigureInterceptors()
    .ConfigureDatabase(builder.Configuration)
    .ConfigureRepositories()
    .ConfigureServices();

var app = builder.Build();

app.UseFastEndpoints();

app.Run();
