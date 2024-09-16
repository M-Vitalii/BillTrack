namespace BillTrack.Core.Interfaces.Services;

public interface IFileUploader
{
    Task UploadFileToS3(Stream pdfStream, string bucketName, string fileName, Guid entityId);
}