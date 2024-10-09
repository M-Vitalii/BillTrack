using BillTrack.Api.Features.Invoices.Models;
using BillTrack.Core.Contracts.Invoice;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Core.Models.WebApi;
using FastEndpoints;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Invoices.Endpoints;

public class GetAllInvoices : Endpoint<InvoiceQueryParamsRequest, PagedResult<InvoiceResponse>>
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

    public override async Task HandleAsync(InvoiceQueryParamsRequest r, CancellationToken c)
    {
        var entities = await _webApiService.GetAllInvoicesPagedAsync(
            r.Page, r.PageSize, r.SortByDate, r.FilterByEmployee, r.FilterByMonth, r.FilterByYear);

        Response = new PagedResult<InvoiceResponse>
        {
            Items = entities.Items.Select(entity =>
            {
                var invoiceResponse = _mapper.Map<InvoiceResponse>(entity);
                invoiceResponse.HasInvoiceUrl = !string.IsNullOrEmpty(entity.InvoiceUrl);
                return invoiceResponse;
            }).ToList(),
            PageNumber = entities.PageNumber,
            PageSize = entities.PageSize,
        };

        await SendAsync(Response, cancellation: c);
    }
}