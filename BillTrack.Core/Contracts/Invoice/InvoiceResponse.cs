using BillTrack.Core.Contracts.Employee;

namespace BillTrack.Core.Contracts.Invoice;

public class InvoiceResponse
{
    public Guid Id { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public required EmployeeResponse Employee { get; set; }

    public bool HasInvoiceUrl { get; set; }
}