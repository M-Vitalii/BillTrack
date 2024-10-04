using FastEndpoints;

namespace BillTrack.Api.Features.Projects.Models;

public class ProjectQueryParamsRequest : PaginationRequest
{
    [QueryParam] public string? FilterByName { get; set; }
    [QueryParam] public string? SortByName { get; set; }
}