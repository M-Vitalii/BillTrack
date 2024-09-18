using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using BillTrack.Core.Interfaces.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace BillTrack.Application.Services;

public class SqsPublisher : ISqsPublisher
{
    private readonly IAmazonSQS _sqs;
    private readonly IMemoryCache _memoryCache;
    private readonly IConfiguration _configuration;

    public SqsPublisher(IAmazonSQS sqs, IMemoryCache memoryCache, IConfiguration configuration)
    {
        _sqs = sqs;
        _memoryCache = memoryCache;
        _configuration = configuration;
    }

    public async Task PublishMessageAsync<T>(T message)
    {
        var queueName = _configuration.GetValue<string>("QueueName");

        var queueUrl = await _memoryCache.GetOrCreateAsync(queueName, async entry =>
        {
            entry.Priority = CacheItemPriority.NeverRemove;
            var response = await _sqs.GetQueueUrlAsync(queueName);
            return response.QueueUrl;
        });
        
        var request = new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = JsonSerializer.Serialize(message)
        };

        await _sqs.SendMessageAsync(request);
    }
}