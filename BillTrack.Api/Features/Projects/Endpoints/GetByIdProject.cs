using BillTrack.Core.Contracts.Department;
using BillTrack.Core.Contracts.Employee;
using BillTrack.Core.Contracts.Project;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Projects.Endpoints;

public class GetByIdProject : EndpointWithoutRequest<ProjectResponse>
{
    private readonly IMapper _mapper;
    private readonly IWebApiService _webApiService;
    
    public GetByIdProject(IMapper mapper, IWebApiService webApiService)
    {
        _mapper = mapper;
        _webApiService = webApiService;
    }
    
    public override void Configure()
    {
        Get("projects/{id}");
    }
    
    public override async Task HandleAsync(CancellationToken c)
    {
        var entity = await _webApiService.GetByIdAsync<Project>(Route<Guid>("id"));
        Response = _mapper.Map<ProjectResponse>(entity);
        
        await SendAsync(Response);
    }
}