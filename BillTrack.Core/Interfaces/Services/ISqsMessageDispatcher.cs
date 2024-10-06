using BillTrack.Core.Interfaces.Models;

namespace BillTrack.Core.Interfaces.Services;

public interface ISqsMessageDispatcher
{
    Task DispatchMessage<T>(string messageBody) where T : IMessage;
}