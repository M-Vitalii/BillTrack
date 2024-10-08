using System.Collections.Concurrent;
using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using BillTrack.Core.Interfaces.Models;
using BillTrack.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace BillTrack.Application.Services;

public class SqsPublisher : ISqsPublisher
{
    private readonly IAmazonSQS _sqs;
    private readonly ConcurrentDictionary<string, string> _cachedQueueUrl;

    public SqsPublisher(IAmazonSQS sqs)
    {
        _sqs = sqs;
        _cachedQueueUrl = new ConcurrentDictionary<string, string>();
    }

    public async Task PublishMessageAsync<T>(string queueName, T message) where T : IMessage
    {
        string queueUrl;
        try
        {
            queueUrl = await GetQueueUrlAsync(queueName);
        }
        catch (QueueDoesNotExistException ex)
        {
            throw;
        }

        var request = new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = JsonSerializer.Serialize(message),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                {
                    "MessageType",
                    new MessageAttributeValue
                    {
                        DataType = "String",
                        StringValue = message.MessageType
                    }
                }
            }
        };

        try
        {
            await _sqs.SendMessageAsync(request);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task<string> GetQueueUrlAsync(string queueName)
    {
        if (_cachedQueueUrl.TryGetValue(queueName, out var cachedUrl))
        {
            return cachedUrl;
        }

        try
        {
            var response = await _sqs.GetQueueUrlAsync(queueName);
            var queueUrl = response.QueueUrl;
            _cachedQueueUrl[queueName] = queueUrl;
            return queueUrl;
        }
        catch (QueueDoesNotExistException)
        {
            _cachedQueueUrl.TryRemove(queueName, out _);
            throw;
        }
    }
}