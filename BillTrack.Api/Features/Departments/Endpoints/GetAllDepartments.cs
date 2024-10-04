using BillTrack.Api.Features.Departments.Models;
using BillTrack.Core.Contracts.Department;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Core.Models.WebApi;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Departments.Endpoints;

public class GetAllDepartments : Endpoint<DepartmentQueryParamsRequest, PagedResult<DepartmentResponse>>
{
    private readonly IWebApiService _webApiService;
    private readonly IMapper _mapper;
        
    public GetAllDepartments(IWebApiService webApiService, IMapper mapper)
    {
        _webApiService = webApiService;
        _mapper = mapper;
    }
        
    public override void Configure()
    {
        Get("departments");
    }
        
    public override async Task HandleAsync(DepartmentQueryParamsRequest r, CancellationToken c)
    {
        var entities = await _webApiService.GetAllDepartmentsPagedAsync(
            r.Page, r.PageSize, r.SortByName, r.FilterByName);
            
        Response = new PagedResult<DepartmentResponse>
        {
            Items = _mapper.Map<List<DepartmentResponse>>(entities.Items),
            PageNumber = entities.PageNumber,
            PageSize = entities.PageSize,
        };
            
        await SendAsync(Response, cancellation: c);
    }
}