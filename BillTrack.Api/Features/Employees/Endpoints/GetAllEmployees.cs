using BillTrack.Api.Features.Employees.Models;
using BillTrack.Core.Contracts.Employee;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Core.Models.WebApi;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Employees.Endpoints;

public class GetAllEmployees : Endpoint<EmployeeQueryParamsRequest, PagedResult<EmployeeResponse>>
{
    private readonly IWebApiService _webApiService;
    private readonly IMapper _mapper;
        
    public GetAllEmployees(IWebApiService webApiService, IMapper mapper)
    {
        _webApiService = webApiService;
        _mapper = mapper;
    }
        
    public override void Configure()
    {
        Get("employees");
    }
        
    public override async Task HandleAsync(EmployeeQueryParamsRequest r, CancellationToken c)
    {
        var entities = await _webApiService.GetAllEmployeesPagedAsync(
            r.Page, r.PageSize, r.SortByName, r.FilterByFirstName, r.FilterByLastName,
            r.FilterByDepartment, r.FilterByProject);
            
        Response = new PagedResult<EmployeeResponse>
        {
            Items = _mapper.Map<List<EmployeeResponse>>(entities.Items),
            PageNumber = entities.PageNumber,
            PageSize = entities.PageSize,
        };
            
        await SendAsync(Response, cancellation: c);
    }
}