using System.Net;
using System.Net.Mail;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.S3;
using BillTrack.Core.Contracts.SqsMessages;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Core.Models;
using BillTrack.Emailer.Configurations;
using FluentEmail.Core;
using FluentEmail.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace BillTrack.Emailer;

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
        
        services.Configure<AwsSettings>(configuration.GetRequiredSection(AwsSettings.SectionName));
        services.Configure<EmailSettings>(configuration.GetRequiredSection(EmailSettings.SectionName));

        var emailSettings = configuration.GetRequiredSection(EmailSettings.SectionName).Get<EmailSettings>()!;

        services.AddAWSService<IAmazonS3>();

        services.ConfigureServices();
        
        var smtp = new SmtpClient
        {
            Host = emailSettings.EmailHost,
            Port = emailSettings.EmailSmtpPort,
            EnableSsl = true,
            UseDefaultCredentials = false,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = new NetworkCredential(
                emailSettings.SenderEmail, 
                emailSettings.SenderPassword)
        };

        Email.DefaultSender = new SmtpSender(smtp);
    }
}