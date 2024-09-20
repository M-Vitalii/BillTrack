using BillTrack.Core.Interfaces.Models;

namespace BillTrack.Core.Interfaces.Services;

public interface IMessageHandler<T>
{
    Task HandleMessageAsync(T message);
}