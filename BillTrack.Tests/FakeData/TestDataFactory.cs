using BillTrack.Core.Models.Worker;
using BillTrack.Domain.Entities;
using Bogus;

namespace BillTrack.Tests.FakeData;

public class TestDataFactory
{
    private readonly Faker<Employee> _employeeFaker;
    private readonly Faker<Workday> _workdayFaker;
    private readonly Faker<Invoice> _invoiceFaker;
    private readonly Faker<EmployeeWorkSummary> _employeeWorkSummaryFaker;

    public TestDataFactory()
    {
        _employeeFaker = new Faker<Employee>()
            .RuleFor(e => e.Id, f => Guid.NewGuid())
            .RuleFor(e => e.Email, f => f.Internet.Email())
            .RuleFor(e => e.Firstname, f => f.Name.FirstName())
            .RuleFor(e => e.Lastname, f => f.Name.LastName())
            .RuleFor(e => e.Salary, f => f.Finance.Amount(200, 5000))
            .RuleFor(e => e.DepartmentId, f => Guid.NewGuid())
            .RuleFor(e => e.ProjectId, f => Guid.NewGuid())
            .RuleFor(e => e.Department, f => new Department { Name = f.Commerce.Department() })
            .RuleFor(e => e.Project, f => new Project { Name = f.Commerce.ProductName() });

        _workdayFaker = new Faker<Workday>()
            .RuleFor(w => w.Date, f => DateOnly.FromDateTime(f.Date.Recent(30)))
            .RuleFor(w => w.Hours, f => f.Random.Decimal(1, 12))
            .RuleFor(w => w.EmployeeId, f => Guid.NewGuid());

        _invoiceFaker = new Faker<Invoice>()
            .RuleFor(i => i.Id, f => Guid.NewGuid())
            .RuleFor(i => i.Month, f => f.Random.Int(1, 12))
            .RuleFor(i => i.Year, f => f.Random.Int(2000, DateTime.Now.Year))
            .RuleFor(i => i.EmployeeId, f => Guid.NewGuid())
            .RuleFor(i => i.InvoiceUrl, f => f.Internet.Url())
            .RuleFor(i => i.Employee, f => _employeeFaker.Generate());
        
        _employeeWorkSummaryFaker = new Faker<EmployeeWorkSummary>()
            .RuleFor(ews => ews.Email, f => f.Internet.Email())
            .RuleFor(ews => ews.FullName, f => $"{f.Name.FirstName()} {f.Name.LastName()}")
            .RuleFor(ews => ews.ProjectName, f => f.Commerce.ProductName())
            .RuleFor(ews => ews.DepartmentName, f => f.Commerce.Department())
            .RuleFor(ews => ews.TotalHoursWorked, f => f.Random.Int(1, 160))
            .RuleFor(ews => ews.HourlyRate, f => f.Finance.Amount(20, 100))
            .RuleFor(ews => ews.CalculatedSalary, (f, ews) => ews.TotalHoursWorked * ews.HourlyRate);
    }

    public Employee GenerateEmployee() => _employeeFaker.Generate();
    public Workday GenerateWorkday() => _workdayFaker.Generate();
    public Invoice GenerateInvoice() => _invoiceFaker.Generate();
    public EmployeeWorkSummary GenerateEmployeeWorkSummary() => _employeeWorkSummaryFaker.Generate();
}
