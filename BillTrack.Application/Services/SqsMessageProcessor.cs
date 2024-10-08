using BillTrack.Core.Contracts.SqsMessages;
using BillTrack.Core.Interfaces.Services;

namespace BillTrack.Application.Services;

public class SqsMessageProcessor : IMessageProcessor
{
    private readonly ISqsMessageDispatcher _sqsMessageDispatcher;
    private readonly Dictionary<string, Func<string, Task>> _handlers;

    public SqsMessageProcessor(ISqsMessageDispatcher sqsMessageDispatcher)
    {
        _sqsMessageDispatcher = sqsMessageDispatcher;
        _handlers = new Dictionary<string, Func<string, Task>>
        {
            { nameof(GeneratedInvoice), _sqsMessageDispatcher.DispatchMessage<GeneratedInvoice> },
            { nameof(CreatedInvoice), _sqsMessageDispatcher.DispatchMessage<CreatedInvoice> },
        };
    }

    public Task ProcessMessage(string messageType, string messageBody)
    {
        if (_handlers.TryGetValue(messageType, out var handler))
        {
            return handler(messageBody);
        }

        throw new InvalidOperationException($"No handler found for message type: {messageType}");
    }
}
