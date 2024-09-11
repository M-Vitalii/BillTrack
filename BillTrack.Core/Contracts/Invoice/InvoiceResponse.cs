namespace BillTrack.Core.Contracts.Invoice;

public record InvoiceResponse(Guid Id, int Month, int Year, Guid EmployeeId);