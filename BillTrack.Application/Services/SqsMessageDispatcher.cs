using System.Text.Json;
using BillTrack.Core.Contracts.SqsMessages;
using BillTrack.Core.Exceptions;
using BillTrack.Core.Interfaces.Models;
using BillTrack.Core.Interfaces.Services;

namespace BillTrack.Application.Services;

public class SqsMessageDispatcher : ISqsMessageDispatcher
{
    private readonly IMessageHandler<CreatedInvoice> _createdInvoiceMessageHandler;

    public SqsMessageDispatcher(IMessageHandler<CreatedInvoice> createdInvoiceMessageHandler)
    {
        _createdInvoiceMessageHandler = createdInvoiceMessageHandler;
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
        switch (message)
        {
            case CreatedInvoice invoice:
                await _createdInvoiceMessageHandler.HandleMessageAsync(invoice);
                break;
            default:
                throw new InvalidOperationException($"Unsupported message object: {message.GetType().Name}");
        }
    }
}