using AutoMapper.QueryableExtensions;
using BillTrack.Core.Contracts.Employee;
using BillTrack.Core.Contracts.Project;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Projects.Endpoints;

public class GetAllProjects : EndpointWithoutRequest<IQueryable<ProjectResponse>>
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
        Get("employees");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(CancellationToken c)
    {
        var entities = _webApiService.GetAll<Project>();
        
        Response = entities.ProjectTo<ProjectResponse>(_mapper.ConfigurationProvider);
        
        await SendAsync(Response);
    }
}