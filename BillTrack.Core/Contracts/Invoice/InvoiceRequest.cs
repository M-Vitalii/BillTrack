namespace BillTrack.Core.Contracts.Invoice;

public record InvoiceRequest(int Month, int Year, Guid EmployeeId);