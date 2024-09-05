using BillTrack.Core.Contracts;
using BillTrack.Core.Contracts.Project;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Core.Models;
using BillTrack.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Projects.Endpoints;

public class GetAllProjects : Endpoint<PaginationRequest, PagedResult<ProjectResponse>>
{
    private readonly IWebApiService _webApiService;
    private readonly IMapper _mapper;
    
    public GetAllProjects(IWebApiService webApiService, IMapper mapper)
    {
        _webApiService = webApiService;
        _mapper = mapper;
    }
    
    public override void Configure()
    {
        Get("projects");
    }
    
    public override async Task HandleAsync(PaginationRequest r, CancellationToken c)
    {
        var entities = await _webApiService.GetAllPagedAsync<Project>(r.Page, r.PageSize);
        
        Response = new PagedResult<ProjectResponse>
        {
            Items = _mapper.Map<List<ProjectResponse>>(entities.Items),
            PageNumber = entities.PageNumber,
            PageSize = entities.PageSize,
        };
        
        await SendAsync(Response, cancellation: c);
    }
}