using FastEndpoints;

namespace BillTrack.Api.Features.Departments.Models;

public class DepartmentQueryParamsRequest : PaginationRequest
{
    [QueryParam] public string? FilterByName { get; set; }
    [QueryParam] public string? SortByName { get; set; }
}