using FastEndpoints;

namespace BillTrack.Api.Features;

public class PaginationRequest
{
    [QueryParam] public int Page { get; set; } = 1;

    [QueryParam] public int PageSize { get; set; } = 10;
}