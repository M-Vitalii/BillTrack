using BillTrack.Api.Features.Invoices.Models;
using BillTrack.Core.Contracts.Invoice;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Core.Models;
using BillTrack.Core.Models.WebApi;
using FastEndpoints;
using Microsoft.Extensions.Options;
using IMapper = AutoMapper.IMapper;

namespace BillTrack.Api.Features.Invoices.Endpoints;

public class GetAllInvoices : Endpoint<InvoiceQueryParamsRequest, PagedResult<InvoiceResponse>>
    {
        private readonly IWebApiService _webApiService;
        private readonly IS3FileService _s3FileService;
        private readonly IOptions<AwsSettings> _awsSettings;
        private readonly IMapper _mapper;
        
        public GetAllInvoices(IWebApiService webApiService, IS3FileService s3FileService, 
            IMapper mapper, IOptions<AwsSettings> awsSettings)
        {
            _webApiService = webApiService;
            _s3FileService = s3FileService;
            _mapper = mapper;
            _awsSettings = awsSettings;
        }
        
        public override void Configure()
        {
            Get("invoices");
        }
        
        public override async Task HandleAsync(InvoiceQueryParamsRequest r, CancellationToken c)
        {
            var entities = await _webApiService.GetAllInvoicesPagedAsync(
                r.Page, r.PageSize, r.SortByDate, r.FilterByEmployee, r.FilterByMonth, r.FilterByYear);
            
            var invoiceResponses = new List<InvoiceResponse>();
            
            foreach (var entity in entities.Items)
            {
                var invoiceResponse = _mapper.Map<InvoiceResponse>(entity);
                
                if (!string.IsNullOrEmpty(invoiceResponse.InvoiceUrl))
                {
                    var objectKey = ExtractObjectKeyFromUrl(invoiceResponse.InvoiceUrl);
                    invoiceResponse.InvoiceUrl = await _s3FileService.GetPresignedUrl(_awsSettings.Value.InvoiceBucketName, objectKey);
                }
                
                invoiceResponses.Add(invoiceResponse);
            }
            
            Response = new PagedResult<InvoiceResponse>
            {
                Items = invoiceResponses,
                PageNumber = entities.PageNumber,
                PageSize = entities.PageSize,
            };
            
            await SendAsync(Response, cancellation: c);
        }
        
        private string ExtractObjectKeyFromUrl(string url)
        {
            return url.Split('/').Last();
        }
    }