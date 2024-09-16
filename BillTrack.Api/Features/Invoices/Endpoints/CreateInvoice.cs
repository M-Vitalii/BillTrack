using BillTrack.Application.Services;
using BillTrack.Core.Contracts.Invoice;
using BillTrack.Core.Contracts.SqsMessages;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Invoices.Endpoints;

public class CreateInvoice : Endpoint<InvoiceRequest, InvoiceResponse>
{
    private readonly IMapper _mapper;
    private readonly IWebApiService _webApiService;

    public CreateInvoice(IMapper mapper, IWebApiService webApiService)
    {
        _mapper = mapper;
        _webApiService = webApiService;
    }

    public override void Configure()
    {
        Post("invoices");
    }

    public override async Task HandleAsync(InvoiceRequest r, CancellationToken c)
    {
        var newEntity = await _webApiService.CreateAsync(_mapper.Map<Invoice>(r));
        Response = _mapper.Map<InvoiceResponse>(newEntity);

        var message = new CreatedInvoice
        {
            InvoiceId = newEntity.Id
        };
        
        await _webApiService.PublishSqsMessageAsync(message);

        await SendAsync(Response);
    }
}