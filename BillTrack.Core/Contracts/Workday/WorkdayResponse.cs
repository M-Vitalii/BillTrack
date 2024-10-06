using BillTrack.Core.Contracts.Employee;

namespace BillTrack.Core.Contracts.Workday;

public record WorkdayResponse(Guid Id, DateOnly Date, decimal Hours, EmployeeResponse Employee);