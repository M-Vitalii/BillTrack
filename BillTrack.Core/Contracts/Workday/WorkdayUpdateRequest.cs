namespace BillTrack.Core.Contracts.Workday;

public record WorkdayUpdateRequest(Guid Id, DateOnly Date, decimal Hours, Guid EmployeeId);