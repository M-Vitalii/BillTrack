using System.Text.Json;
using BillTrack.Core.Contracts.Department;
using BillTrack.Core.Contracts.Employee;
using BillTrack.Core.Contracts.Invoice;
using BillTrack.Core.Contracts.Project;
using BillTrack.Core.Contracts.SqsMessages;
using BillTrack.Core.Exceptions;
using BillTrack.Core.Interfaces.Repositories;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Core.Models;
using BillTrack.Domain.Entities;
using BillTrack.Worker.Configurations;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Context;

namespace BillTrack.Worker.Handlers;

public class CreatedInvoiceMessageHandler : IMessageHandler<CreatedInvoice>
{
    private readonly IPdfGenerator _pdfGenerator;
    private readonly IS3FileService _is3FileService;
    private readonly IGenericRepository<Invoice> _invoiceRepository;
    private readonly IOptions<AwsSettings> _awsConfiguration;
    private readonly ISqsPublisher _sqsPublisher;

    public CreatedInvoiceMessageHandler(
        IPdfGenerator pdfGenerator,
        IS3FileService is3FileService,
        IGenericRepository<Invoice> invoiceRepository,
        IOptions<AwsSettings> awsConfiguration,
        ISqsPublisher sqsPublisher)
    {
        _pdfGenerator = pdfGenerator;
        _is3FileService = is3FileService;
        _invoiceRepository = invoiceRepository;
        _awsConfiguration = awsConfiguration;
        _sqsPublisher = sqsPublisher;
    }

    public async Task HandleMessageAsync(CreatedInvoice invoiceMessage)
    {
        using (LogContext.PushProperty("InvoiceId", invoiceMessage.InvoiceId))
        {
            var fileName = $"invoice-{invoiceMessage.InvoiceId}.pdf";

            try
            {
                var invoice = await GetInvoiceWithEmployee(invoiceMessage.InvoiceId);

                LogContext.PushProperty("invoice", invoice == null ? "null" : invoice);

                if (invoice == null)
                {
                    Log.Logger.Error($"invoice - {invoice}");
                    throw new NotFoundException("Invalid invoice");
                }

                var invoiceUrl = GenerateInvoiceUrl(fileName);

                await GenerateAndUploadPdf(invoiceMessage.InvoiceId, fileName);
                await UpdateInvoiceUrl(invoice, invoiceUrl);

                LogContext.PushProperty("awsSettings", JsonSerializer.Serialize(_awsConfiguration.Value));
                
                var generatedMessage = CreateGeneratedInvoiceMessage(invoice, fileName, $"{invoice.Employee.Firstname} {invoice.Employee.Lastname}");
                await _sqsPublisher.PublishMessageAsync(_awsConfiguration.Value.EmailQueue, generatedMessage);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to process invoice");
                throw;
            }
        }
    }

    private async Task GenerateAndUploadPdf(Guid invoiceId, string fileName)
    {
        var pdfStream = await _pdfGenerator.GeneratePdfStream(invoiceId);
        await _is3FileService.UploadFileToS3(pdfStream, _awsConfiguration.Value.InvoiceBucketName, fileName,
            "application/pdf");
    }

    private async Task<Invoice?> GetInvoiceWithEmployee(Guid invoiceId)
    {
        return await _invoiceRepository.GetByIdAsync(invoiceId, i => i.Employee);
    }

    private string GenerateInvoiceUrl(string fileName)
    {
        return
            $"https://{_awsConfiguration.Value.InvoiceBucketName}.s3.{_awsConfiguration.Value.Region}.amazonaws.com/{fileName}";
    }

    private async Task UpdateInvoiceUrl(Invoice invoice, string invoiceUrl)
    {
        invoice.InvoiceUrl = invoiceUrl;

        using (LogContext.PushProperty("Invoice", invoice))
        {
            await _invoiceRepository.UpdateAsync(invoice);
        }
    }

    private GeneratedInvoice CreateGeneratedInvoiceMessage(Invoice invoice, string fileName, string employeeFullName)
    {
        return new GeneratedInvoice
        {
            FileName = fileName,
            EmailTo = invoice.Employee.Email,
            EmployeeFullName = employeeFullName
        };
    }
}