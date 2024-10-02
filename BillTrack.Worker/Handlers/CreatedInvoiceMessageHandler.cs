using BillTrack.Core.Contracts.SqsMessages;
using BillTrack.Core.Interfaces.Repositories;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using BillTrack.Worker.Configurations;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Context;

namespace BillTrack.Worker.Handlers;

public class CreatedInvoiceMessageHandler : IMessageHandler<CreatedInvoice>
{
    private readonly IPdfGenerator _pdfGenerator;
    private readonly IS3FileUploader _s3FileUploader;
    private readonly IGenericRepository<Invoice> _invoiceRepository;
    private readonly IOptions<AwsSettings> _appConfiguration;

    public CreatedInvoiceMessageHandler(
        IPdfGenerator pdfGenerator,
        IS3FileUploader s3FileUploader,
        IGenericRepository<Invoice> invoiceRepository,
        IOptions<AwsSettings> appConfiguration)
    {
        _pdfGenerator = pdfGenerator;
        _s3FileUploader = s3FileUploader;
        _invoiceRepository = invoiceRepository;
        _appConfiguration = appConfiguration;
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
        await _s3FileUploader.UploadFileToS3(pdfStream, _appConfiguration.Value.BucketName, fileName, "application/pdf");
    }

    private async Task UpdateInvoiceUrl(Guid invoiceId, string fileName)
    {
        var invoiceUrl = $"https://{_appConfiguration.Value.BucketName}.s3.{_appConfiguration.Value.AwsRegion}.amazonaws.com/{fileName}";
        var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);

        invoice.InvoiceUrl = invoiceUrl;

        await _invoiceRepository.UpdateAsync(invoice);
    }
}