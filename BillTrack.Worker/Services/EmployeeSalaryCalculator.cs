using BillTrack.Core.Exceptions;
using BillTrack.Core.Interfaces.Repositories;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Core.Models.Worker;
using BillTrack.Domain.Entities;

namespace BillTrack.Worker.Services;

public class EmployeeSalaryCalculator : IEmployeeSalaryCalculator
{
    private readonly IGenericRepository<Employee> _employeeRepository;

    public EmployeeSalaryCalculator(IGenericRepository<Employee> employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<EmployeeWorkSummary> CalculateEmployeeSalaryAsync(Invoice invoice)
    {
        var firstDayOfMonth = new DateOnly(invoice.Year, invoice.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        var employee = await GetEmployeeAsync(invoice.EmployeeId);
        var weekdaysInMonth = GetWeekdaysInMonth(firstDayOfMonth, lastDayOfMonth);

        var totalHoursWorked = CalculateWorkHours(employee, firstDayOfMonth, lastDayOfMonth);
        var hourlyRate = CalculateHourlyRate(employee.Salary, weekdaysInMonth);

        var calculatedSalary = CalculateSalary(hourlyRate, totalHoursWorked);

        return CreateEmployeeWorkSummary(employee, totalHoursWorked, hourlyRate, calculatedSalary);
    }

    private async Task<Employee> GetEmployeeAsync(Guid employeeId)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId,
            e => e.Department,
            e => e.Project,
            e => e.Workdays);

        if (employee == null)
        {
            throw new NotFoundException("Employee not found.");
        }

        return employee;
    }

    private decimal CalculateHourlyRate(decimal salary, int weekdaysInMonth)
    {
        var defaultWorkHours = weekdaysInMonth * 8;
        return salary / defaultWorkHours;
    }

    private decimal CalculateWorkHours(Employee employee, DateOnly firstDayOfMonth, DateOnly lastDayOfMonth)
    {
        return employee.Workdays
            .Where(w => w.Date >= firstDayOfMonth && w.Date <= lastDayOfMonth)
            .Sum(w => w.Hours);
    }

    private decimal CalculateSalary(decimal hourlyRate, decimal totalHoursWorked)
    {
        var calculatedSalary = hourlyRate * totalHoursWorked;
        return Math.Round(calculatedSalary, 2, MidpointRounding.AwayFromZero);
    }

    private EmployeeWorkSummary CreateEmployeeWorkSummary(Employee employee, decimal totalHoursWorked, decimal hourlyRate, decimal calculatedSalary)
    {
        return new EmployeeWorkSummary
        {
            Email = employee.Email,
            FullName = $"{employee.Firstname} {employee.Lastname}",
            ProjectName = employee.Project.Name,
            DepartmentName = employee.Department.Name,
            TotalHoursWorked = totalHoursWorked,
            HourlyRate = hourlyRate,
            CalculatedSalary = calculatedSalary
        };
    }

    private int GetWeekdaysInMonth(DateOnly firstDay, DateOnly lastDay)
    {
        return Enumerable.Range(0, lastDay.DayNumber - firstDay.DayNumber + 1)
            .Select(offset => firstDay.AddDays(offset))
            .Count(day => day.DayOfWeek is >= DayOfWeek.Monday and <= DayOfWeek.Friday);
    }
}
