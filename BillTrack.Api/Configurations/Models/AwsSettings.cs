namespace BillTrack.Api.Configurations;

public class AwsSettings
{
    public const string SectionName = "AWS";
    public string WorkerQueueName { get; set; } = "";
}