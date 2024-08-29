using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;

namespace BillTrack.Api.Features.Invoices.Endpoints;

public class DeleteInvoice : EndpointWithoutRequest
{
    private readonly IWebApiService _webApiService;
    
    public DeleteInvoice(IWebApiService webApiService)
    {
        _webApiService = webApiService;
    }
    
    public override void Configure()
    {
        Delete("invoices/{id}");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(CancellationToken c)
    {
        await _webApiService.DeleteAsync<Invoice>(Route<Guid>("id"));
        
        await SendResultAsync(TypedResults.NoContent());
    }
}