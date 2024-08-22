using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;

namespace BillTrack.Api.Features.Employees.Endpoints;

public class DeleteEmployee : EndpointWithoutRequest
{
    private readonly IWebApiService _webApiService;
    
    public DeleteEmployee(IMapper mapper, IWebApiService webApiService)
    {
        _webApiService = webApiService;
    }
    
    public override void Configure()
    {
        Delete("employees/{id}");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(CancellationToken c)
    {
        await _webApiService.DeleteAsync<Employee>(Route<Guid>("id"));
        
        await SendResultAsync(TypedResults.NoContent());
    }
}