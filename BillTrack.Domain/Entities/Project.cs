namespace BillTrack.Domain.Entities;

public class Project
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}