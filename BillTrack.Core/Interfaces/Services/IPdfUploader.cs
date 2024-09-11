namespace BillTrack.Core.Interfaces.Services;

public interface IPdfUploader
{
    Task UploadPdfToS3(Stream pdfStream, string bucketName, string fileName, Guid invoiceId);
}