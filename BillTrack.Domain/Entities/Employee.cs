namespace BillTrack.Domain.Entities;

public class Employee
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public decimal Salary { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid ProjectId { get; set; }
    
    public required Department Department { get; set; }
    public required Project Project { get; set; }

    public ICollection<Workday> Workdays { get; set; } = new List<Workday>();
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}