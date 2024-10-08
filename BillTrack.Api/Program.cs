global using FluentValidation;
using Amazon.S3;
using Amazon.SQS;
using BillTrack.Api;
using BillTrack.Api.Configurations;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(
            policy  =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });
}

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
builder.Services.AddAWSService<IAmazonS3>();

if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors();
}

await app.UseDatabaseMigrations();

app.Seed();

app.UseGlobalExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(c => { c.Endpoints.RoutePrefix = "api"; });

app.UseSwaggerGen();

app.Run();