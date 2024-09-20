using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using BillTrack.Core.Interfaces.Models;
using BillTrack.Core.Interfaces.Services;

namespace BillTrack.Application.Services;

public class SqsPublisher : ISqsPublisher
{
    private readonly IAmazonSQS _sqs;
    private IDictionary<string, string> _cachedQueueUrl;


    public SqsPublisher(IAmazonSQS sqs)
    {
        _sqs = sqs;
        _cachedQueueUrl = new Dictionary<string, string>();
    }

    public async Task PublishMessageAsync<T>(string queueName, T message) where T : IMessage
    {
        if (!_cachedQueueUrl.TryGetValue(queueName, out var queueUrl))
        {
            var getQueueUrlResponse = await _sqs.GetQueueUrlAsync(queueName);
            queueUrl = getQueueUrlResponse.QueueUrl;
            _cachedQueueUrl[queueName] = queueUrl;
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

        await _sqs.SendMessageAsync(request);
    }
}