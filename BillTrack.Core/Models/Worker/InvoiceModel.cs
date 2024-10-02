using BillTrack.Domain.Entities;

namespace BillTrack.Core.Models.Worker;

public class InvoiceModel
{
    public Guid InvoiceId { get; set; }
    public DateTime IssueDate { get; set; }
    public EmployeeWorkSummary EmployeeWorkSummary { get; set; }
}