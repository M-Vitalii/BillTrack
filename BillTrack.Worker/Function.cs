using System.Reflection;
using System.Text.Json;
using Amazon;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Lambda.SQSEvents;
using Amazon.S3;
using BillTrack.Core.Contracts.SqsMessages;
using BillTrack.Core.Interfaces.Repositories;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using BillTrack.Worker.Configurations;
using BillTrack.Worker.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.AwsCloudWatch;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace BillTrack.Worker;

public class Function
{
    private readonly IFileGenerator _fileGenerator;
    private readonly IFileUploader _fileUploader;
    private readonly IGenericRepository<Invoice> _invoiceRepository;

    private readonly AppConfiguration _appConfiguration;

    public Function()
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        _fileGenerator = serviceProvider.GetRequiredService<IFileGenerator>();
        _fileUploader = serviceProvider.GetRequiredService<IFileUploader>();
        _invoiceRepository = serviceProvider.GetRequiredService<IGenericRepository<Invoice>>();
        _appConfiguration = serviceProvider.GetRequiredService<IOptions<AppConfiguration>>().Value;
    }

    public async Task FunctionHandler(SQSEvent evnt)
    {
        foreach (var message in evnt.Records)
        {
            await ProcessMessageAsync(message);
        }
    }

    private async Task ProcessMessageAsync(SQSEvent.SQSMessage message)
    {
        var invoice = JsonSerializer.Deserialize<CreatedInvoice>(message.Body);

        var fileName = $"invoice-{invoice.InvoiceId}.pdf";

        try
        {
            var pdfStream = await _fileGenerator.GenerateFile(invoice.InvoiceId);

            try
            {
                await _fileUploader.UploadFileToS3(pdfStream, _appConfiguration.BucketName, fileName,
                    invoice.InvoiceId);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(
                    ex,
                    "Failed to upload PDF for InvoiceId: {InvoiceId} with sqs message: {@SqsMessage}",
                    invoice.InvoiceId, message);
                throw;
            }
        }
        catch (Exception ex)
        {
            Log.Logger.Error(
                ex,
                "Failed to generate PDF for InvoiceId: {InvoiceId} with sqs message: {@SqsMessage}",
                invoice.InvoiceId, message);
            throw;
        }

        await UpdateInvoiceUrl(invoice.InvoiceId, fileName);

        await Task.CompletedTask;
    }


    private async Task UpdateInvoiceUrl(Guid invoiceId, string fileName)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);

        invoice.InvoiceUrl =
            $"https://{_appConfiguration.BucketName}.s3.{_appConfiguration.AwsRegion}.amazonaws.com/{fileName}";

        await _invoiceRepository.UpdateAsync(invoice);
    }

    private void ConfigureServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        services.Configure<AppConfiguration>(config =>
        {
            config.BucketName = configuration["BUCKET_NAME"];
            config.AwsRegion = configuration["AWS_REGION"];
            config.ConnectionString = configuration["CONNECTION_STRING"];
        });

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .WriteTo.Console(new JsonFormatter())
            .CreateLogger();

        services
            .ConfigureDatabase(configuration["CONNECTION_STRING"])
            .ConfigureRepositories();

        services.AddTransient<IEmployeeSalaryCalculator, EmployeeSalaryCalculator>();
        services.AddTransient<IFileGenerator, InvoicePdfGenerator>();
        services.AddTransient<IFileUploader, InvoicePdfUploader>();

        services.AddAWSService<IAmazonS3>();
    }
}