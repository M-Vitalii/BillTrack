using System.Text.Json;
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
using Microsoft.Extensions.Options;


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

        _fileGenerator = serviceProvider.GetService<IFileGenerator>()!;
        _fileUploader = serviceProvider.GetService<IFileUploader>()!;
        _invoiceRepository = serviceProvider.GetService<IGenericRepository<Invoice>>()!;
        _appConfiguration = serviceProvider.GetRequiredService<IOptions<AppConfiguration>>().Value;
    }

    public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        foreach (var message in evnt.Records)
        {
            await ProcessMessageAsync(message, context);
        }
    }

    private async Task ProcessMessageAsync(
        SQSEvent.SQSMessage message,
        ILambdaContext context)
    {
        var logger = context.Logger;
        var invoice = JsonSerializer.Deserialize<CreatedInvoice>(message.Body);
        
        var fileName = $"invoice-{invoice.InvoiceId}.pdf";
        
        try
        {
            var pdfStream = await _fileGenerator.GenerateFile(invoice.InvoiceId);

            try
            {
                await _fileUploader.UploadFileToS3(pdfStream, _appConfiguration.BucketName, fileName, invoice.InvoiceId);
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to upload PDF for InvoiceId: {invoice.InvoiceId}");
                throw;
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Failed to generate PDF for InvoiceId: {invoice.InvoiceId}");
            throw;
        }

        await UpdateInvoiceUrl(invoice.InvoiceId, fileName);

        await Task.CompletedTask;
    }
    
        
    private async Task UpdateInvoiceUrl(Guid invoiceId, string fileName)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);

        invoice.InvoiceUrl = $"https://{_appConfiguration.BucketName}.s3.{_appConfiguration.AwsRegion}.amazonaws.com/{fileName}";
        
        await _invoiceRepository.UpdateAsync(invoice);
    }

    private void ConfigureServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();
        
        services.Configure<AppConfiguration>(configuration.Bind);
        
        services
            .ConfigureDatabase(configuration["CONNECTION_STRING"])
            .ConfigureRepositories();

        services.AddTransient<IFileGenerator, InvoicePdfGenerator>();
        services.AddTransient<IFileUploader, InvoicePdfUploader>();

        services.AddAWSService<IAmazonS3>();
    }
}