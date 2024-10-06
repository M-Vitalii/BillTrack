namespace BillTrack.Core.Contracts.Employee;

public record EmployeeUpdateRequest(
    Guid Id,
    string Email,
    string Firstname,
    string Lastname,
    decimal Salary,
    Guid DepartmentId,
    Guid ProjectId);