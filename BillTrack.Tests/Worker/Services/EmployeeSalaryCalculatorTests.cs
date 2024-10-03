using System.Linq.Expressions;
using BillTrack.Core.Exceptions;
using BillTrack.Core.Interfaces.Repositories;
using BillTrack.Domain.Entities;
using BillTrack.Tests.FakeData;
using BillTrack.Worker.Services;
using Moq;

namespace BillTrack.Tests.Worker.Services;

public class EmployeeSalaryCalculatorTests
{
    private readonly Mock<IGenericRepository<Employee>> _employeeRepositoryMock;
    private readonly EmployeeSalaryCalculator _calculator;

    private readonly TestDataFactory _testData;

    public EmployeeSalaryCalculatorTests()
    {
        _employeeRepositoryMock = new Mock<IGenericRepository<Employee>>();
        _calculator = new EmployeeSalaryCalculator(_employeeRepositoryMock.Object);

        _testData = new TestDataFactory();
    }
    
    [Fact]
    public async Task CalculateEmployeeSalaryAsync_ShouldReturnWorkSummary_WhenEmployeeExists()
    {
        // Arrange
        var invoiceId = new Guid();
        var employee = new Employee
        {
            Id = invoiceId,
            Firstname = "John",
            Lastname = "Doe",
            Salary = 4000m,
            Department = new Department
            {
                Name = "IT"
            },
            Project = new Project
            {
                Name = "Project X"
            },
            Email = "email@email.com"
        };
        
        var invoice = new Invoice { Id = invoiceId, EmployeeId = Guid.NewGuid(), Year = 2024, Month = 10, Employee = employee};
        employee.Workdays = new List<Workday>()
        {
            new Workday { Date = new DateOnly(2024, 10, 1), Hours = 8, Employee = employee },
            new Workday { Date = new DateOnly(2024, 10, 2), Hours = 8, Employee = employee }
        };

        _employeeRepositoryMock.Setup(repo => repo.GetByIdAsync(invoice.EmployeeId, It.IsAny<Expression<Func<Employee, object>>[]>()))
            .ReturnsAsync(employee);

        // Act
        var result = await _calculator.CalculateEmployeeSalaryAsync(invoice);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(4000m / (23*8), result.HourlyRate);
    }
    
    [Fact]
    public async Task CalculateEmployeeSalaryAsync_ShouldThrowNotFoundException_WhenEmployeeDoesNotExist()
    {
        // Arrange
        var invoice = _testData.GenerateInvoice();
        _employeeRepositoryMock.Setup(repo => repo.GetByIdAsync(invoice.EmployeeId, It.IsAny<Expression<Func<Employee, object>>[]>()))
            .ReturnsAsync((Employee)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _calculator.CalculateEmployeeSalaryAsync(invoice));
    }
    
    [Fact]
    public async Task CalculateEmployeeSalaryAsync_ShouldReturnZeroHours_WhenNoWorkdaysInMonth()
    {
        // Arrange
        var invoiceId = new Guid();
        
        var employee = new Employee
        {
            Id = invoiceId,
            Firstname = "John",
            Lastname = "Doe",
            Salary = 4000m,
            Workdays = new List<Workday>(),
            Department = new Department
            {
                Name = "IT"
            },
            Project = new Project
            {
                Name = "Project X"
            },
            Email = "email@email.com"
        };
        var invoice = new Invoice {Id = invoiceId, EmployeeId = Guid.NewGuid(), Year = 2024, Month = 10, Employee = employee};

        _employeeRepositoryMock.Setup(repo => repo.GetByIdAsync(invoice.EmployeeId, It.IsAny<Expression<Func<Employee, object>>[]>()))
            .ReturnsAsync(employee);

        // Act
        var result = await _calculator.CalculateEmployeeSalaryAsync(invoice);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.TotalHoursWorked);
        Assert.Equal(0, result.CalculatedSalary);
    }
    
    [Fact]
    public async Task CalculateEmployeeSalaryAsync_ShouldReturnZeroSalary_WhenNoHoursWorked()
    {
        // Arrange
        var invoiceId = new Guid();
        var employee = new Employee
        {
            Id = invoiceId,
            Firstname = "John",
            Lastname = "Doe",
            Salary = 4000m,
            Department = new Department
            {
                Name = "IT"
            },
            Project = new Project
            {
                Name = "Project X"
            },
            Email = "email@email.com"
        };
        
        var invoice = new Invoice { Id = invoiceId, EmployeeId = Guid.NewGuid(), Year = 2024, Month = 10, Employee = employee};
        employee.Workdays = new List<Workday>()
        {
            new Workday { Date = new DateOnly(2024, 10, 1), Hours = 0, Employee = employee },
            new Workday { Date = new DateOnly(2024, 10, 2), Hours = 0, Employee = employee }
        };

        _employeeRepositoryMock.Setup(repo => repo.GetByIdAsync(invoice.EmployeeId, It.IsAny<Expression<Func<Employee, object>>[]>()))
            .ReturnsAsync(employee);

        // Act
        var result = await _calculator.CalculateEmployeeSalaryAsync(invoice);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.TotalHoursWorked);
        Assert.Equal(4000m / (23 * 8), result.HourlyRate);
        Assert.Equal(0, result.CalculatedSalary);
    }
}