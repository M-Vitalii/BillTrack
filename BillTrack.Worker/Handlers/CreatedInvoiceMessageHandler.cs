using BillTrack.Core.Contracts.SqsMessages;
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

    public CreatedInvoiceMessageHandler(
        IPdfGenerator pdfGenerator,
        IS3FileService is3FileService,
        IGenericRepository<Invoice> invoiceRepository,
        IOptions<AwsSettings> awsConfiguration)
    {
        _pdfGenerator = pdfGenerator;
        _is3FileService = is3FileService;
        _invoiceRepository = invoiceRepository;
        _awsConfiguration = awsConfiguration;
    }

    public async Task HandleMessageAsync(CreatedInvoice invoice)
    {
        using (LogContext.PushProperty("InvoiceId", invoice.InvoiceId))
        {
            var fileName = $"invoice-{invoice.InvoiceId}.pdf";

            try
            {
                await GenerateAndUploadPdf(invoice.InvoiceId, fileName);
                await UpdateInvoiceUrl(invoice.InvoiceId, fileName);
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
        await _is3FileService.UploadFileToS3(pdfStream, _awsConfiguration.Value.InvoiceBucketName, fileName, "application/pdf");
    }

    private async Task UpdateInvoiceUrl(Guid invoiceId, string fileName)
    {
        var invoiceUrl = $"https://{_awsConfiguration.Value.InvoiceBucketName}.s3.{_awsConfiguration.Value.Region}.amazonaws.com/{fileName}";
        var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);

        invoice.InvoiceUrl = invoiceUrl;

        using (LogContext.PushProperty("Invoice", invoice))
        {
            await _invoiceRepository.UpdateAsync(invoice);
        }
    }
}