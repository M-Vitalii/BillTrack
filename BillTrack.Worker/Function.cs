using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Lambda.SQSEvents;
using Amazon.S3;
using Amazon.SQS;
using BillTrack.Core.Contracts.SqsMessages;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Core.Models;
using BillTrack.Worker.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Context;
using Serilog.Formatting.Json;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace BillTrack.Worker;

public class Function
{
    private readonly IMessageProcessor _messageProcessor;
    
    public Function()
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        _messageProcessor = serviceProvider.GetRequiredService<IMessageProcessor>();
    }

    public async Task FunctionHandler(SQSEvent evnt)
    {
        foreach (var message in evnt.Records)
        {
            var messageType = message.MessageAttributes["MessageType"].StringValue;
            await _messageProcessor.ProcessMessage(messageType, message.Body);
        }
    }

    private void ConfigureServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        services.Configure<AwsSettings>(configuration.GetSection(AwsSettings.SectionName));

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .WriteTo.Console(new JsonFormatter())
            .Enrich.FromLogContext()
            .CreateLogger();

        services
            .ConfigureDatabase(configuration["CONNECTION_STRING"])
            .ConfigureRepositories()
            .ConfigureServices();

        services.AddAWSService<IAmazonS3>();
        services.AddAWSService<IAmazonSQS>();
    }
}