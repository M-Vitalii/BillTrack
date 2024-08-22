using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;

namespace BillTrack.Api.Features.Workdays.Endpoints;

public class DeleteWorkday : EndpointWithoutRequest
{
    private readonly IWebApiService _webApiService;
    
    public DeleteWorkday(IMapper mapper, IWebApiService webApiService)
    {
        _webApiService = webApiService;
    }
    
    public override void Configure()
    {
        Delete("workdays/{id}");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(CancellationToken c)
    {
        await _webApiService.DeleteAsync<Workday>(Route<Guid>("id"));
        
        await SendResultAsync(TypedResults.NoContent());
    }
}