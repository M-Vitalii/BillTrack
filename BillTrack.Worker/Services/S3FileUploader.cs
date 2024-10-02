using Amazon.S3;
using Amazon.S3.Model;
using BillTrack.Core.Interfaces.Repositories;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;

namespace BillTrack.Worker.Services;

public class S3FileUploader : IS3FileUploader
{
    private readonly IAmazonS3 _s3Client;
    
    public S3FileUploader(IAmazonS3 s3Client)
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
}