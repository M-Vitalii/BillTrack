using BillTrack.Core.Interfaces.Services;
using BillTrack.Core.Models;
using BillTrack.Domain.Entities;
using FastEndpoints;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace BillTrack.Api.Features.Invoices.Endpoints;

public class GetInvoicePreSignedUrl : EndpointWithoutRequest<string>
{
    private readonly IWebApiService _webApiService;
    private readonly IS3FileService _s3FileService;
    private readonly AwsSettings _awsSettings;
    private readonly IMemoryCache _cache;

    public GetInvoicePreSignedUrl(IWebApiService webApiService, IS3FileService s3FileService, IMemoryCache cache, IOptions<AwsSettings> awsSettings)
    {
        _webApiService = webApiService;
        _s3FileService = s3FileService;
        _cache = cache;
        _awsSettings = awsSettings.Value;
    }

    public override void Configure()
    {
        Get("invoices/{id}/pre-signed-url");
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        var invoiceId = Route<Guid>("id");

        var entity = await _webApiService.GetByIdAsync<Invoice>(invoiceId);

        if (!_cache.TryGetValue(invoiceId, out string? preSignedUrl))
        {
            preSignedUrl = await _s3FileService.GetPresignedUrl(_awsSettings.InvoiceBucketName, entity.InvoiceUrl.Split("/").Last());

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(14)
            };

            _cache.Set(invoiceId, preSignedUrl, cacheOptions);
        }

        await SendAsync(preSignedUrl, cancellation: c);
    }
}
