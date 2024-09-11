using Amazon.SQS;
using Amazon.SQS.Model;
using System.Text.Json;
using BillTrack.Core.Interfaces.Services;

namespace BillTrack.Application.Services;

public class SqsPublisher : ISqsPublisher
{
    private readonly IAmazonSQS _sqs;

    public SqsPublisher(IAmazonSQS sqs)
    {
        _sqs = sqs;
    }

    public async Task PublishAsync<T>(string queueName, T message)
    {
        var queueUrl = await _sqs.GetQueueUrlAsync(queueName);

        var request = new SendMessageRequest
        {
            QueueUrl = queueUrl.QueueUrl,
            MessageBody = JsonSerializer.Serialize(message)
        };

        await _sqs.SendMessageAsync(request);
    }

}