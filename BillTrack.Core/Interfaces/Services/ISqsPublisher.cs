namespace BillTrack.Core.Interfaces.Services;

public interface ISqsPublisher
{
    Task PublishMessageAsync<T>(T message);
}