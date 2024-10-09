using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using BillTrack.Core.Interfaces.Models;
using BillTrack.Core.Interfaces.Services;

namespace BillTrack.Application.Services;

public class SqsPublisher : ISqsPublisher
{
    private readonly IAmazonSQS _sqs;
    private readonly Dictionary<string, string> _cachedQueueUrl;

    public SqsPublisher(IAmazonSQS sqs)
    {
        _sqs = sqs;
        _cachedQueueUrl = new Dictionary<string, string>();
    }

    public async Task PublishMessageAsync<T>(string queueName, T message) where T : IMessage
    {
        var queueUrl = await GetQueueUrlAsync(queueName);

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

        await _sqs.SendMessageAsync(request);
    }

    private async Task<string> GetQueueUrlAsync(string queueName)
    {
        GetQueueUrlResponse? response;
        
        if (_cachedQueueUrl.TryGetValue(queueName, out var cachedUrl))
        {
            return cachedUrl;
        }

        try
        {
            response = await _sqs.GetQueueUrlAsync(queueName);
        }
        catch (QueueDoesNotExistException)
        {
            _cachedQueueUrl.Remove(queueName);
            throw;
        }
        
        if (null == response || string.IsNullOrEmpty(response.QueueUrl)) {
            throw new QueueDoesNotExistException("Error during a queue URL resolution");
        }
        
        var queueUrl = response.QueueUrl;
        _cachedQueueUrl[queueName] = queueUrl;
        
        return queueUrl;
    }
}