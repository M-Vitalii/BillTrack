using BillTrack.Core.Interfaces.Models;
using BillTrack.Core.Interfaces.Services;

namespace BillTrack.Core.Factories;

public interface IMessageHandlerFactory
{
    IMessageHandler<T> GetHandler<T>() where T : IMessage;
}