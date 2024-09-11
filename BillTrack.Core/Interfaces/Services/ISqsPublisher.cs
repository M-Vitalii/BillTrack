namespace BillTrack.Core.Interfaces.Services;

public interface ISqsPublisher
{
    Task PublishAsync<T>(string queueName, T message);
}