using System;
using System.Linq.Expressions;
using BillTrack.Application.Helpers;
using BillTrack.Domain.Entities;

namespace BillTrack.Application.Filters;

public static class EntityFilter
{
    private static Expression<Func<T, bool>> BuildFilter<T>(params (bool condition, Expression<Func<T, bool>> predicate)[] filters)
    {
        Expression<Func<T, bool>>? combinedFilter = null;

        foreach (var (condition, predicate) in filters)
        {
            if (condition)
            {
                combinedFilter = combinedFilter == null ? 
                    predicate : 
                    combinedFilter.And(predicate);
            }
        }

        return combinedFilter ?? PredicateBuilder.True<T>();
    }

    public static Expression<Func<Workday, bool>> BuildWorkdayFilter(DateOnly? date, Guid? employeeId)
    {
        return BuildFilter<Workday>(
            (date.HasValue, w => w.Date == date.Value),
            (employeeId.HasValue, w => w.EmployeeId == employeeId.Value)
        );
    }

    public static Expression<Func<Employee, bool>> BuildEmployeeFilter(
        string? firstName, 
        string? lastName,
        Guid? departmentId, 
        Guid? projectId)
    {
        return BuildFilter<Employee>(
            (!string.IsNullOrEmpty(firstName), e => e.Firstname.Contains(firstName)),
            (!string.IsNullOrEmpty(lastName), e => e.Lastname.Contains(lastName)),
            (departmentId.HasValue, e => e.DepartmentId == departmentId.Value),
            (projectId.HasValue, e => e.ProjectId == projectId.Value)
        );
    }
    
    public static Expression<Func<Department, bool>> BuildDepartmentFilter(string? name)
    {
        return BuildFilter<Department>(
            (!string.IsNullOrEmpty(name), d => d.Name == name)
        );
    }
    
    public static Expression<Func<Project, bool>> BuildProjectFilter(string? name)
    {
        return BuildFilter<Project>(
            (!string.IsNullOrEmpty(name), p => p.Name == name)
        );
    }
    
    public static Expression<Func<Invoice, bool>> BuildInvoiceFilter(Guid? employeeId, int? month, int? year)
    {
        return BuildFilter<Invoice>(
            (employeeId.HasValue, i => i.EmployeeId == employeeId.Value),
            (month.HasValue, i => i.Month == month.Value),
            (year.HasValue, i => i.Year == year.Value)
        );
    }
}