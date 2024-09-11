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
    private readonly ISqsPublisher _sqsPublisher;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CreateInvoice> _logger;

    public CreateInvoice(IMapper mapper, IWebApiService webApiService, ISqsPublisher sqsPublisher,
        IConfiguration configuration, ILogger<CreateInvoice> logger)
    {
        _mapper = mapper;
        _webApiService = webApiService;
        _sqsPublisher = sqsPublisher;
        _configuration = configuration;
        _logger = logger;
    }

    public override void Configure()
    {
        Post("invoices");
    }

    public override async Task HandleAsync(InvoiceRequest r, CancellationToken c)
    {
        var newEntity = await _webApiService.CreateAsync(_mapper.Map<Invoice>(r));
        Response = _mapper.Map<InvoiceResponse>(newEntity);
        
        await _sqsPublisher.PublishAsync(
            _configuration.GetValue<string>("QueueName")!,
            new CreatedInvoice
            {
                InvoiceId = newEntity.Id
            });

        await SendAsync(Response);
    }
}