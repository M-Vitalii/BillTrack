namespace BillTrack.Core.Contracts.Workday;

public record WorkdayRequest(DateOnly Date, decimal Hours, Guid EmployeeId);