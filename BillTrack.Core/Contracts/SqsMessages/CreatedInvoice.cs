using System.Text.Json.Serialization;

namespace BillTrack.Core.Contracts.SqsMessages;

public class CreatedInvoice
{
    [JsonPropertyName("invoiceId")]
    public Guid InvoiceId { get; set; }
}