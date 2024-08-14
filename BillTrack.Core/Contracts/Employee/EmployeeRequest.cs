namespace BillTrack.Core.Contracts.Employee;

public record EmployeeRequest(
    string Email,
    string Firstname,
    string Lastname,
    decimal Salary,
    Guid DepartmentId,
    Guid ProjectId);