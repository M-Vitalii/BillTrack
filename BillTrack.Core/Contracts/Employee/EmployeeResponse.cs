namespace BillTrack.Core.Contracts.Employee;

public record EmployeeResponse(
    Guid Id,
    string Email,
    string Firstname,
    string Lastname,
    Guid DepartmentId,
    Guid ProjectId);