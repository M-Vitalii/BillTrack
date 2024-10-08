namespace BillTrack.Domain.Entities;

public class Invoice : AuditableEntity
{
    public int Month { get; set; }
    public int Year { get; set; }
    public Guid EmployeeId { get; set; }

    public string InvoiceUrl { get; set; } = "";

    public required Employee Employee { get; set; }
    
    public override string ToString()
    {
        return $"Invoice for {Employee.Firstname} {Employee.Lastname} (Employee ID: {EmployeeId}) - " +
               $"Month: {Month}, Year: {Year}, Invoice URL: {InvoiceUrl}";
    }
}