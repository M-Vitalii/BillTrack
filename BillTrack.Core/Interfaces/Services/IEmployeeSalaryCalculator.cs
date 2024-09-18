using BillTrack.Core.Models.Worker;
using BillTrack.Domain.Entities;

namespace BillTrack.Core.Interfaces.Services;

public interface IEmployeeSalaryCalculator
{
    Task<EmployeeWorkSummary> CalculateEmployeeSalaryAsync(Invoice invoice);
}