using System.Text.Json;
using BillTrack.Core.Contracts.SqsMessages;
using BillTrack.Core.Exceptions;
using BillTrack.Core.Factories;
using BillTrack.Core.Interfaces.Models;
using BillTrack.Core.Interfaces.Services;

namespace BillTrack.Application.Services;

public class SqsMessageDispatcher : ISqsMessageDispatcher
{
    private readonly IMessageHandlerFactory _messageHandlerFactory;

    public SqsMessageDispatcher(IMessageHandlerFactory messageHandlerFactory)
    {
        _messageHandlerFactory = messageHandlerFactory;
    }

    public async Task DispatchMessage<T>(string messageBody) where T : IMessage
    {
        try
        {
            var message = JsonSerializer.Deserialize<T>(messageBody);
            await ProcessMessage(message);
        }
        catch (Exception exception)
        {
            throw new JsonGeneralException("Failed to deserialize message", exception);
        }
    }

    private async Task ProcessMessage<T>(T message) where T : IMessage
    {
        var handler = _messageHandlerFactory.GetHandler<T>();
        await handler.HandleMessageAsync(message);
    }
}
