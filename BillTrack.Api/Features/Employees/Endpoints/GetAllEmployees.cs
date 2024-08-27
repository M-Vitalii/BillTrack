using BillTrack.Core.Contracts;
using BillTrack.Core.Contracts.Employee;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Core.Models;
using BillTrack.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Employees.Endpoints;

public class GetAllEmployees : Endpoint<PaginationRequest, PagedResult<EmployeeResponse>>
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
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(PaginationRequest r, CancellationToken c)
    {
        var entities = await _webApiService.GetAllPagedAsync<Employee>(r.Page, r.PageSize);
        
        Response = new PagedResult<EmployeeResponse>
        {
            Items = _mapper.Map<List<EmployeeResponse>>(entities.Items),
            PageNumber = entities.PageNumber,
            PageSize = entities.PageSize,
        };
        
        await SendAsync(Response, cancellation: c);
    }
}