using BillTrack.Domain.Entities;

namespace BillTrack.Worker.Models;

public class InvoiceModel
{
    public Guid InvoiceId { get; set; }
    public DateTime IssueDate { get; set; }
    public Employee Employee { get; set; }
}