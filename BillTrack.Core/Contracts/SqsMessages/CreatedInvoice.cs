using System.Text.Json.Serialization;
using BillTrack.Core.Interfaces.Models;

namespace BillTrack.Core.Contracts.SqsMessages;

public class CreatedInvoice : IMessage
{
    [JsonPropertyName("invoiceId")]
    public Guid InvoiceId { get; set; }

    public string MessageType => nameof(CreatedInvoice);
}