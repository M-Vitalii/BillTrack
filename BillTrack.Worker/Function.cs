using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Lambda.SQSEvents;
using Amazon.S3;
using BillTrack.Core.Contracts.SqsMessages;
using BillTrack.Core.Interfaces.Services;
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
    private readonly Dictionary<string, Func<string, Task>> _handlers;
    
    public Function()
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var sqsMessageDispatcher = serviceProvider.GetRequiredService<ISqsMessageDispatcher>();
        
        _handlers = new Dictionary<string, Func<string, Task>>
        {
            { nameof(CreatedInvoice), sqsMessageDispatcher.DispatchMessage<CreatedInvoice> },
        };
    }

    public async Task FunctionHandler(SQSEvent evnt)
    {
        foreach (var message in evnt.Records)
        {
            using (LogContext.PushProperty("SqsMessage", message))
            {
                var messageAttributes = message.MessageAttributes;
                if (messageAttributes != null && messageAttributes.TryGetValue("MessageType", out var attribute))
                {
                    var messageType = attribute.StringValue;

                    await ProcessMessageBasedOnType(messageType, message.Body);
                }
                else
                {
                    throw new InvalidOperationException("Message type attribute is missing");
                }
            }
        }
    }
    
    
    private Task ProcessMessageBasedOnType(string messageType, string messageBody)
    {
        if (_handlers.TryGetValue(messageType, out var handler))
        {
            return handler(messageBody);
        }

        throw new InvalidOperationException($"No handler found for message type: {messageType}");
    }

    private void ConfigureServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        services.Configure<AwsSettings>(config =>
        {
            config.AwsRegion = configuration["AWS_REGION"];
            config.BucketName = configuration["BUCKET_NAME"];
        });

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
    }
}