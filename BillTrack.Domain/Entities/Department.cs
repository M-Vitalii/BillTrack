namespace BillTrack.Domain.Entities;

public class Department : BaseEntity
{
    public required string Name { get; set; }

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}