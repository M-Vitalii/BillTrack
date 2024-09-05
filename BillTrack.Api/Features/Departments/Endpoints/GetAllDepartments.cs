using AutoMapper.QueryableExtensions;
using BillTrack.Core.Contracts;
using BillTrack.Core.Contracts.Department;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Core.Models;
using BillTrack.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Departments.Endpoints;

public class GetAllDepartments : Endpoint<PaginationRequest, PagedResult<DepartmentResponse>>
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
    
    public override async Task HandleAsync(PaginationRequest r, CancellationToken c)
    {
        var entities = await _webApiService.GetAllPagedAsync<Department>(r.Page, r.PageSize);
        
        Response = new PagedResult<DepartmentResponse>
        {
            Items = _mapper.Map<List<DepartmentResponse>>(entities.Items),
            PageNumber = entities.PageNumber,
            PageSize = entities.PageSize,
        };
        
        await SendAsync(Response, cancellation: c);
    }
}