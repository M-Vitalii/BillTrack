using BillTrack.Core.Contracts.Department;
using BillTrack.Core.Contracts.Project;

namespace BillTrack.Core.Contracts.Employee;

public record EmployeeResponse(
    Guid Id,
    string Email,
    string Firstname,
    string Lastname,
    decimal Salary,
    DepartmentResponse Department,
    ProjectResponse Project);