namespace BillTrack.Core.Models;

public class AwsSettings
{
    public const string SectionName = "AWS";
    public string Region { get; set; } = "";
    public string WorkerQueueName { get; set; } = "";
    public string InvoiceBucketName { get; set; } = "";
    public string EmailQueue { get; set; } = "";
}