namespace BillTrack.Domain.Entities;

public class Workday : AuditableEntity
{
    public DateOnly Date { get; set; }
    public decimal Hours { get; set; }
    public Guid EmployeeId { get; set; }
    
    public required Employee Employee { get; set; }
}