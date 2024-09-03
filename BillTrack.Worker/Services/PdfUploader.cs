using Amazon.S3;
using Amazon.S3.Model;
using BillTrack.Core.Interfaces.Repositories;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;

namespace BillTrack.Worker.Services;

public class PdfUploader : IPdfUploader
{
    private readonly IAmazonS3 _s3Client;
    private readonly IGenericRepository<Invoice> _invoiceRepository;
    
    public PdfUploader(IAmazonS3 s3Client, IGenericRepository<Invoice> invoiceRepository)
    {
        _s3Client = s3Client;
        _invoiceRepository = invoiceRepository;
    }

    public async Task UploadPdfToS3(Stream pdfStream, string bucketName, string fileName, Guid invoiceId)
    {
        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = fileName,
            InputStream = pdfStream,
            ContentType = "application/pdf"
        };

        await _s3Client.PutObjectAsync(request);

        await UpdateInvoiceUrl(invoiceId, bucketName, fileName);
    }
    
    private async Task UpdateInvoiceUrl(Guid invoiceId, string bucketName, string fileName)
    {
        var region = Environment.GetEnvironmentVariable("AWS_REGION");
        var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);

        invoice.InvoiceUrl = $"https://{bucketName}.s3.{region}.amazonaws.com/{fileName}";
        await _invoiceRepository.UpdateAsync(invoice);
    }
}