using System.Text.Json;
using BillTrack.Core.Contracts.SqsMessages;
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
        var message = JsonSerializer.Deserialize<T>(messageBody);

        if (message == null)
        {
            throw new ArgumentNullException("Failed to deserialize message");
        }
        
        await ProcessMessage(message);
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