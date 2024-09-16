using Microsoft.Extensions.Configuration;

namespace BillTrack.Worker.Configurations;

public class AppConfiguration
{
    public string? BucketName { get; set; }
    public string? ConnectionString { get; set; }
    public string? AwsRegion { get; set; }
}