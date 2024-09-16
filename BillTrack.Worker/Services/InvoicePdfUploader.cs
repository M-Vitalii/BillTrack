using Amazon.S3;
using Amazon.S3.Model;
using BillTrack.Core.Interfaces.Repositories;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;

namespace BillTrack.Worker.Services;

public class InvoicePdfUploader : IFileUploader
{
    private readonly IAmazonS3 _s3Client;
    
    public InvoicePdfUploader(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    public async Task UploadFileToS3(Stream pdfStream, string bucketName, string fileName, Guid invoiceId)
    {
        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = fileName,
            InputStream = pdfStream,
            ContentType = "application/pdf"
        };

        await _s3Client.PutObjectAsync(request);
    }
}