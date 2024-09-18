using BillTrack.Core.Contracts;
using BillTrack.Core.Contracts.Invoice;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Core.Models;
using BillTrack.Core.Models.WebApi;
using BillTrack.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Invoices.Endpoints;

public class GetAllInvoices : Endpoint<PaginationRequest, PagedResult<InvoiceResponse>>
{
    private readonly IWebApiService _webApiService;
    private readonly IMapper _mapper;
    
    public GetAllInvoices(IWebApiService webApiService, IMapper mapper)
    {
        _webApiService = webApiService;
        _mapper = mapper;
    }
    
    public override void Configure()
    {
        Get("invoices");
    }
    
    public override async Task HandleAsync(PaginationRequest r, CancellationToken c)
    {
        var entities = await _webApiService.GetAllPagedAsync<Invoice>(r.Page, r.PageSize);
        
        Response = new PagedResult<InvoiceResponse>
        {
            Items = _mapper.Map<List<InvoiceResponse>>(entities.Items),
            PageNumber = entities.PageNumber,
            PageSize = entities.PageSize,
        };
        
        await SendAsync(Response, cancellation: c);
    }
}