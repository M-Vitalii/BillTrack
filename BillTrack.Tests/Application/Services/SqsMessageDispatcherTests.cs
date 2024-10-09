using System.Text.Json;
using BillTrack.Application.Services;
using BillTrack.Core.Contracts.SqsMessages;
using BillTrack.Core.Exceptions;
using BillTrack.Core.Factories;
using BillTrack.Core.Interfaces.Models;
using BillTrack.Core.Interfaces.Services;
using Moq;

namespace BillTrack.Tests.Application.Services;

public class SqsMessageDispatcherTests
{
    private readonly Mock<IMessageHandlerFactory> _messageHandlerFactory;
    private readonly SqsMessageDispatcher _sqsMessageDispatcher;

    public SqsMessageDispatcherTests()
    {
        _messageHandlerFactory = new Mock<IMessageHandlerFactory>();
        _sqsMessageDispatcher = new SqsMessageDispatcher(_messageHandlerFactory.Object);
    }

    [Fact]
    public async Task DispatchMessage_ShouldCallHandleMessageAsync_ForCreatedInvoice()
    {
        // Arrange
        var invoiceGuidId = Guid.NewGuid();
        var createdInvoice = new CreatedInvoice { InvoiceId = invoiceGuidId };
        var messageBody = JsonSerializer.Serialize(createdInvoice);

        var mockCreatedInvoiceHandler = new Mock<IMessageHandler<CreatedInvoice>>();
    
        _messageHandlerFactory
            .Setup(factory => factory.GetHandler<CreatedInvoice>())
            .Returns(mockCreatedInvoiceHandler.Object);

        // Act
        await _sqsMessageDispatcher.DispatchMessage<CreatedInvoice>(messageBody);

        // Assert
        mockCreatedInvoiceHandler.Verify(x => x.HandleMessageAsync(It.Is<CreatedInvoice>(i => i.InvoiceId == invoiceGuidId)), Times.Once);
    }


    [Fact]
    public async Task DispatchMessage_ShouldThrowJsonGeneralException_WhenDeserializationFails()
    {
        // Arrange
        var invalidMessageBody = "InvalidMessageFormat";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<JsonGeneralException>(() => _sqsMessageDispatcher.DispatchMessage<CreatedInvoice>(invalidMessageBody));
        Assert.IsType<JsonException>(exception.InnerException);
    }

    [Fact]
    public async Task DispatchMessage_ShouldThrowJsonGeneralException_ForUnsupportedMessageType()
    {
        // Arrange
        var unsupportedMessage = new SomeUnsupportedMessage();
        var messageBody = JsonSerializer.Serialize(unsupportedMessage);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<JsonGeneralException>(() => _sqsMessageDispatcher.DispatchMessage<SomeUnsupportedMessage>(messageBody));
        Assert.IsType<InvalidOperationException>(exception.InnerException);
    }
}

public class SomeUnsupportedMessage : IMessage
{
    public string MessageId { get; set; } = "";
    public string MessageType => nameof(SomeUnsupportedMessage);
}
