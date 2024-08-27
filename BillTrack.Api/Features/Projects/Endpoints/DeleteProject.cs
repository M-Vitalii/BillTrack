using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;

namespace BillTrack.Api.Features.Projects.Endpoints;

public class DeleteProject : EndpointWithoutRequest
{
    private readonly IWebApiService _webApiService;
    
    public DeleteProject(IWebApiService webApiService)
    {
        _webApiService = webApiService;
    }
    
    public override void Configure()
    {
        Delete("projects/{id}");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(CancellationToken c)
    {
        await _webApiService.DeleteAsync<Project>(Route<Guid>("id"));
        
        await SendResultAsync(TypedResults.NoContent());
    }
}