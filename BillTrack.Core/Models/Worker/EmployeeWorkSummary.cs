namespace BillTrack.Core.Models.Worker;

public class EmployeeWorkSummary
{
    public required string Email { get; set; }
    public required string FullName { get; set; }
    public required string ProjectName { get; set; }
    public required string DepartmentName { get; set; }
    public decimal TotalHoursWorked { get; set; }
    public decimal HourlyRate { get; set; }
    public decimal CalculatedSalary { get; set; }
}