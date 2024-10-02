namespace BillTrack.Core.Contracts.Invoice;

public record InvoiceUpdateRequest(Guid Id, int Month, int Year, Guid EmployeeId);