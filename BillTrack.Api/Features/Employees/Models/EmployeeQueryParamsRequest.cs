using FastEndpoints;

namespace BillTrack.Api.Features.Employees.Models;

public class EmployeeQueryParamsRequest : PaginationRequest
{
    [QueryParam] public string? FilterByFirstName { get; set; }
    [QueryParam] public string? FilterByLastName { get; set; }
    [QueryParam] public Guid? FilterByDepartment { get; set; }
    [QueryParam] public Guid? FilterByProject { get; set; }
    [QueryParam] public string? SortByName { get; set; }
}