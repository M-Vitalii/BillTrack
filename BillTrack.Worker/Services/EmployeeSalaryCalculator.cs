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

        var weekdaysInMonth = GetWeekdaysInMonth(firstDayOfMonth, lastDayOfMonth);

        var defaultWorkHours = weekdaysInMonth * 8;

        var employee = await _employeeRepository.GetByIdAsync(invoice.EmployeeId,
            e => e.Department,
            e => e.Project,
            e => e.Workdays);

        if (employee == null)
        {
            throw new NotFoundException("Employee not found.");
        }

        var totalHoursWorked = employee.Workdays
            .Where(w => w.Date >= firstDayOfMonth && w.Date <= lastDayOfMonth)
            .Sum(w => w.Hours);

        var hourlyRate = employee.Salary / defaultWorkHours;

        var calculatedSalary = hourlyRate * totalHoursWorked;
        calculatedSalary = Math.Round(calculatedSalary, 2, MidpointRounding.AwayFromZero);


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