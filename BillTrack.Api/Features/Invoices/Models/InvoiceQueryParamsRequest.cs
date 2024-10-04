using FastEndpoints;

namespace BillTrack.Api.Features.Invoices.Models;

public class InvoiceQueryParamsRequest : PaginationRequest
{
    [QueryParam] public Guid? FilterByEmployee { get; set; }
    [QueryParam] public int? FilterByMonth { get; set; }
    [QueryParam] public int? FilterByYear { get; set; }
    [QueryParam] public string? SortByDate { get; set; }
}