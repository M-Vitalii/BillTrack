namespace BillTrack.Core.Interfaces.Services;

public interface IS3FileService
{
    Task UploadFileToS3(Stream fileStream, string bucketName, string fileName, string contentType);
    Task<string?> GetPresignedUrl(string bucketName, string objectKey, double expireMinutes = 15);
    Task<Stream> GetObjectAsync(string bucketName, string objectKey);
}