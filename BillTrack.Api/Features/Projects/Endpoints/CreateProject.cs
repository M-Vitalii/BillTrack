using BillTrack.Core.Contracts.Project;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Projects.Endpoints;

public class CreateProject : Endpoint<ProjectRequest, ProjectResponse>
{
    private readonly IMapper _mapper;
    private readonly IWebApiService _webApiService;
    
    public CreateProject(IMapper mapper, IWebApiService webApiService)
    {
        _mapper = mapper;
        _webApiService = webApiService;
    }
    
    public override void Configure()
    {
        Post("projects");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(ProjectRequest r, CancellationToken c)
    {
        var newEntity = await _webApiService.CreateAsync(_mapper.Map<Project>(r));
        Response = _mapper.Map<ProjectResponse>(newEntity);
        
        await SendAsync(Response);
    }
}