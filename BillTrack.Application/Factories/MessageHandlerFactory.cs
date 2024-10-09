using BillTrack.Core.Factories;
using BillTrack.Core.Interfaces.Models;
using BillTrack.Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BillTrack.Application.Factories;

public class MessageHandlerFactory : IMessageHandlerFactory
{
    private readonly IServiceProvider _serviceProvider;

    public MessageHandlerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IMessageHandler<T> GetHandler<T>() where T : IMessage
    {
        var handler = _serviceProvider.GetRequiredService<IMessageHandler<T>>();
        
        if (handler == null)
        {
            throw new InvalidOperationException($"No handler registered for message type {typeof(T).Name}");
        }

        return handler;
    }
}
