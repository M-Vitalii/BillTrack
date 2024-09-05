using BillTrack.Core.Contracts.Invoice;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Invoices.Endpoints;

public class UpdateInvoice : Endpoint<InvoiceRequest>
{
    private readonly IMapper _mapper;
    private readonly IWebApiService _webApiService;
    
    public UpdateInvoice(IMapper mapper, IWebApiService webApiService)
    {
        _mapper = mapper;
        _webApiService = webApiService;
    }
    
    public override void Configure()
    {
        Put("invoices/{id}");
    }
    
    public override async Task HandleAsync(InvoiceRequest r, CancellationToken c)
    {
        await _webApiService.UpdateAsync(Route<Guid>("id"), _mapper.Map<Invoice>(r));
        
        await SendResultAsync(TypedResults.Ok());
    }
}