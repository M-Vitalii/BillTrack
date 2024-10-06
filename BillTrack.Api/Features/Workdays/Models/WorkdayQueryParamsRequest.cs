using BillTrack.Core.Models;
using FastEndpoints;

namespace BillTrack.Api.Features.Workdays.Models;

public class WorkdayQueryParamsRequest : PaginationRequest
{
    [QueryParam] public DateOnly? FilterByDate { get; set; }
    [QueryParam] public Guid? FilterByEmployee { get; set; }
    [QueryParam] public string? SortByDate { get; set; }
}