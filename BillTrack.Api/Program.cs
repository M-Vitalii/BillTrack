using BillTrack.Api.Configurations;
using FastEndpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();

builder.Services.ConfigureDatabase(builder.Configuration);

var app = builder.Build();

app.UseFastEndpoints();

app.Run();
