using Amazon.S3;
using Amazon.S3.Model;
using BillTrack.Core.Interfaces.Services;

namespace BillTrack.Application.Services;

public class S3FileService : IS3FileService
{
    private readonly IAmazonS3 _s3Client;

    public S3FileService(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    public async Task UploadFileToS3(Stream fileStream, string bucketName, string fileName, string contentType)
    {
        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = fileName,
            InputStream = fileStream,
            ContentType = contentType
        };

        await _s3Client.PutObjectAsync(request);
    }

    public async Task<string> GetPresignedUrl(string bucketName, string objectKey, double expireMinutes = 15)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = objectKey,
            Expires = DateTime.UtcNow.AddMinutes(expireMinutes)
        };

        return await _s3Client.GetPreSignedURLAsync(request);
    }

    public async Task<Stream> GetObjectAsync(string bucketName, string objectKey)
    {
        var request = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = objectKey
        };

        var response = await _s3Client.GetObjectAsync(request);

        return response.ResponseStream;
    }
}