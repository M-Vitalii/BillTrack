namespace BillTrack.Domain.Entities;

public class Workday
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Hours { get; set; }
    public Guid EmployeeId { get; set; }
    
    public required Employee Employee { get; set; }
}