using BillTrack.Core.Interfaces.Models;

namespace BillTrack.Core.Interfaces.Services;

public interface ISqsPublisher
{
    Task PublishMessageAsync<T>(string queueName, T message) where T : IMessage;
}