global using FluentValidation;
using Amazon.SQS;
using BillTrack.Api.Configurations;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());

builder.Services
    .AddAuthenticationJwtBearer(s => s.SigningKey = builder.Configuration["JwtSecretKey"])
    .AddAuthorization()
    .AddFastEndpoints()
    .SwaggerDocument();

builder.Logging.AddConsole();

builder.Services
    .ConfigureInterceptors()
    .ConfigureAppSettings(builder.Configuration)
    .ConfigureDatabase(builder.Configuration)
    .ConfigureRepositories()
    .ConfigureServices()
    .ConfigureMappers();

builder.Services.AddMemoryCache();

builder.Services.AddAWSService<IAmazonSQS>();

if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
}

var app = builder.Build();

await app.UseDatabaseMigrations();

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
