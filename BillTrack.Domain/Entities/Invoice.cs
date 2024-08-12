namespace BillTrack.Domain.Entities;

public class Invoice : AuditableEntity
{
    public int Month { get; set; }
    public int Year { get; set; }
    public Guid EmployeeId { get; set; }

    public required Employee Employee { get; set; }
}