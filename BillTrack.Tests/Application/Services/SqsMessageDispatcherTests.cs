using System.Text.Json;
using BillTrack.Application.Services;
using BillTrack.Core.Contracts.SqsMessages;
using BillTrack.Core.Exceptions;
using BillTrack.Core.Interfaces.Models;
using BillTrack.Core.Interfaces.Services;
using Moq;

namespace BillTrack.Tests.Application.Services;

public class SqsMessageDispatcherTests
{
    private readonly Mock<IMessageHandler<CreatedInvoice>> _mockCreatedInvoiceHandler;
    private readonly SqsMessageDispatcher _sqsMessageDispatcher;

    public SqsMessageDispatcherTests()
    {
        _mockCreatedInvoiceHandler = new Mock<IMessageHandler<CreatedInvoice>>();
        _sqsMessageDispatcher = new SqsMessageDispatcher(_mockCreatedInvoiceHandler.Object);
    }

    [Fact]
    public async Task DispatchMessage_ShouldCallHandleMessageAsync_ForCreatedInvoice()
    {
        // Arrange
        var invoiceGuidId = new Guid();
        var createdInvoice = new CreatedInvoice { InvoiceId = invoiceGuidId };
        var messageBody = JsonSerializer.Serialize(createdInvoice);

        // Act
        await _sqsMessageDispatcher.DispatchMessage<CreatedInvoice>(messageBody);

        // Assert
        _mockCreatedInvoiceHandler.Verify(x => x.HandleMessageAsync(It.Is<CreatedInvoice>(i => i.InvoiceId == invoiceGuidId)), Times.Once);
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
