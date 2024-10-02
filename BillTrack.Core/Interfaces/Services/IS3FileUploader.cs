namespace BillTrack.Core.Interfaces.Services;

public interface IS3FileUploader
{
    Task UploadFileToS3(Stream fileStream, string bucketName, string fileName, string contentType);
}