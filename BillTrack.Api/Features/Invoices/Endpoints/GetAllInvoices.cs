using AutoMapper.QueryableExtensions;
using BillTrack.Core.Contracts.Invoice;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Invoices.Endpoints;

public class GetAllInvoices : EndpointWithoutRequest<IQueryable<InvoiceResponse>>
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
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(CancellationToken c)
    {
        var entities = _webApiService.GetAll<Invoice>();
        
        Response = entities.ProjectTo<InvoiceResponse>(_mapper.ConfigurationProvider);
        
        await SendAsync(Response);
    }
}