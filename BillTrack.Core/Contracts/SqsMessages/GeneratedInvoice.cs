using System.Text.Json.Serialization;
using BillTrack.Core.Interfaces.Models;

namespace BillTrack.Core.Contracts.SqsMessages;

public class GeneratedInvoice : IMessage
{
    [JsonPropertyName("fileName")] public string FileName { get; set; } = "";
    [JsonPropertyName("employeeFullName")] public string EmployeeFullName { get; set; } = "";
    [JsonPropertyName("emailTo")] public string EmailTo { get; set; } = "";
    public string MessageType => nameof(GeneratedInvoice);
}