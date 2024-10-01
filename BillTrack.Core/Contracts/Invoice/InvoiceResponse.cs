namespace BillTrack.Core.Contracts.Invoice;

public class InvoiceResponse()
{
    public Guid Id { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public Guid EmployeeId { get; set; }
    public string InvoiceUrl { get; set; } = "";
};