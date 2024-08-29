using BillTrack.Core.Contracts.Department;
using BillTrack.Core.Contracts.Employee;
using BillTrack.Core.Contracts.Project;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Projects.Endpoints;

public class UpdateProject : Endpoint<ProjectRequest>
{
    private readonly IMapper _mapper;
    private readonly IWebApiService _webApiService;
    
    public UpdateProject(IMapper mapper, IWebApiService webApiService)
    {
        _mapper = mapper;
        _webApiService = webApiService;
    }
    
    public override void Configure()
    {
        Put("projects/{id}");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(ProjectRequest r, CancellationToken c)
    {
        await _webApiService.UpdateAsync(Route<Guid>("id"), _mapper.Map<Project>(r));
        
        await SendResultAsync(TypedResults.Ok());
    }
}