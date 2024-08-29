using FastEndpoints;

namespace BillTrack.Core.Contracts;

public class PaginationRequest
{
    [QueryParam] public int Page { get; set; } = 1;

    [QueryParam] public int PageSize { get; set; } = 10;
}