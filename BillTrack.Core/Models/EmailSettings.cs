namespace BillTrack.Core.Models;

public class EmailSettings
{
    public const string SectionName = "Email";
    public string EmailHost { get; set; } = "";
    public int EmailSmtpPort { get; set; } = 0;
    public string SenderEmail { get; set; } = "";
    public string SenderPassword { get; set; } = "";
}