using BillTrack.Core.Contracts.Invoice;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Invoices.Endpoints;

public class GetByIdInvoice : EndpointWithoutRequest<InvoiceResponse>
{
    private readonly IMapper _mapper;
    private readonly IWebApiService _webApiService;
    
    public GetByIdInvoice(IMapper mapper, IWebApiService webApiService)
    {
        _mapper = mapper;
        _webApiService = webApiService;
    }
    
    public override void Configure()
    {
        Get("invoices/{id}");
    }
    
    public override async Task HandleAsync(CancellationToken c)
    {
        var entity = await _webApiService.GetByIdAsync<Invoice>(Route<Guid>("id"));
        Response = _mapper.Map<InvoiceResponse>(entity);
        
        await SendAsync(Response);
    }
}