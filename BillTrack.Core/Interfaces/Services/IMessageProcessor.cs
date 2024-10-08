namespace BillTrack.Core.Interfaces.Services;

public interface IMessageProcessor
{
    Task ProcessMessage(string messageType, string messageBody);
}
