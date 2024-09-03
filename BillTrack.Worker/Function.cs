using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Lambda.SQSEvents;
using Amazon.S3;
using BillTrack.Core.Contracts.SqsMessages;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Worker.Configurations;
using BillTrack.Worker.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace BillTrack.Worker
{
    public class Function
    {
        private readonly IPdfGenerator _pdfGenerator;
        private readonly IPdfUploader _pdfUploader;

        public Function()
        {
            var serviceCollection = new ServiceCollection();
            
            ConfigureServices(serviceCollection);
            
            var serviceProvider = serviceCollection.BuildServiceProvider();
            
            _pdfGenerator = serviceProvider.GetService<IPdfGenerator>()!;
            _pdfUploader = serviceProvider.GetService<IPdfUploader>()!;
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
            var invoice = JsonSerializer.Deserialize<CreatedInvoice>(message.Body);
            
            string bucketName = Environment.GetEnvironmentVariable("BucketName");
            string fileName = $"invoice-{invoice.InvoiceId}.pdf";
            
            var pdfStream = await _pdfGenerator.GeneratePdf(invoice.InvoiceId);
            await _pdfUploader.UploadPdfToS3(pdfStream, bucketName, fileName, invoice.InvoiceId);
            
            await Task.CompletedTask;
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services
                .ConfigureDatabase(Environment.GetEnvironmentVariable("DefaultConnection"))
                .ConfigureRepositories();

            services.AddTransient<IPdfGenerator, PdfGenerator>();
            services.AddTransient<IPdfUploader, PdfUploader>();
            
            services.AddAWSService<IAmazonS3>();
        }
    }
}