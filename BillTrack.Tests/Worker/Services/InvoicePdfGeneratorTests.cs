using System.Linq.Expressions;
using BillTrack.Core.Interfaces.Repositories;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Core.Models.Worker;
using BillTrack.Domain.Entities;
using BillTrack.Tests.FakeData;
using BillTrack.Worker.Services;
using Moq;

namespace BillTrack.Tests.Worker.Services;

public class InvoicePdfGeneratorTests
{
    private readonly Mock<IGenericRepository<Invoice>> _invoiceRepositoryMock;
    private readonly Mock<IEmployeeSalaryCalculator> _employeeSalaryCalculatorMock;
    private readonly InvoicePdfGenerator _pdfGenerator;

    private readonly TestDataFactory _testDataFactory;

    public InvoicePdfGeneratorTests()
    {
        _invoiceRepositoryMock = new Mock<IGenericRepository<Invoice>>();
        _employeeSalaryCalculatorMock = new Mock<IEmployeeSalaryCalculator>();
        _pdfGenerator = new InvoicePdfGenerator(_invoiceRepositoryMock.Object, _employeeSalaryCalculatorMock.Object);
        
        _testDataFactory = new TestDataFactory();
    }

    [Fact]
    public async Task GeneratePdfStream_ValidInvoiceId_ReturnsStream()
    {
        // Arrange
        var invoiceId = Guid.NewGuid();
        var invoice = _testDataFactory.GenerateInvoice();
        var employeeWorkSummary = _testDataFactory.GenerateEmployeeWorkSummary;

        _invoiceRepositoryMock.Setup(repo => repo.GetByIdAsync(
                invoiceId,
                It.IsAny<Expression<Func<Invoice, object>>[]>()))
            .ReturnsAsync(invoice);

        _employeeSalaryCalculatorMock.Setup(calc => calc.CalculateEmployeeSalaryAsync(invoice))
            .ReturnsAsync(employeeWorkSummary);

        // Act
        var result = await _pdfGenerator.GeneratePdfStream(invoiceId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<MemoryStream>(result);
        Assert.True(result.Length > 0);
    }

    [Fact]
    public async Task GeneratePdfStream_InvalidInvoiceId_ThrowsException()
    {
        // Arrange
        var invoiceId = Guid.NewGuid();
        _invoiceRepositoryMock.Setup(repo => repo.GetByIdAsync(
            invoiceId,
            It.IsAny<Expression<Func<Invoice, object>>[]>()));
    
        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => _pdfGenerator.GeneratePdfStream(invoiceId));
    }
    
    [Fact]
    public async Task GeneratePdfStream_SalaryCalculatorReturnsNull_ThrowsException()
    {
        // Arrange
        var invoiceId = Guid.NewGuid();
        var invoice = _testDataFactory.GenerateInvoice();

        _invoiceRepositoryMock.Setup(repo => repo.GetByIdAsync(
                invoiceId,
                It.IsAny<Expression<Func<Invoice, object>>[]>()))
            .ReturnsAsync(invoice);

        _employeeSalaryCalculatorMock.Setup(calc => calc.CalculateEmployeeSalaryAsync(invoice))
            .ReturnsAsync((EmployeeWorkSummary)null);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => _pdfGenerator.GeneratePdfStream(invoiceId));
    }

    [Fact]
    public async Task GeneratePdfStream_IncompleteInvoiceData_ThrowsException()
    {
        // Arrange
        var invoiceId = Guid.NewGuid();

        var incompleteInvoice = _testDataFactory.GenerateInvoice();
        incompleteInvoice.Employee = null;

        _invoiceRepositoryMock.Setup(repo => repo.GetByIdAsync(
                invoiceId,
                It.IsAny<Expression<Func<Invoice, object>>[]>()))
            .ReturnsAsync(incompleteInvoice);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => _pdfGenerator.GeneratePdfStream(invoiceId));
    }
}