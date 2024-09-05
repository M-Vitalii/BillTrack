using BillTrack.Core.Contracts.Department;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;

namespace BillTrack.Api.Features.Departments.Endpoints;

public class DeleteDepartment : EndpointWithoutRequest
{
    private readonly IWebApiService _webApiService;
    
    public DeleteDepartment(IWebApiService webApiService)
    {
        _webApiService = webApiService;
    }
    
    public override void Configure()
    {
        Delete("departments/{id}");
    }
    
    public override async Task HandleAsync(CancellationToken c)
    {
        await _webApiService.DeleteAsync<Department>(Route<Guid>("id"));
        
        await SendResultAsync(TypedResults.NoContent());
    }
}