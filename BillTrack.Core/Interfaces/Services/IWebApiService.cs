using System.Linq.Expressions;
using BillTrack.Core.Interfaces.Models;
using BillTrack.Core.Models;
using BillTrack.Core.Models.WebApi;
using BillTrack.Domain.Entities;

namespace BillTrack.Core.Interfaces.Services;

public interface IWebApiService
{
    Task<T> CreateAsync<T>(T entity) where T : AuditableEntity;
    Task<T> GetByIdAsync<T>(Guid id) where T : AuditableEntity;

    Task<PagedResult<T>> GetAllPagedAsync<T>(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string? sortDirection = SortDirection.Asc) where T : AuditableEntity;
    
    Task UpdateAsync<T>(Guid id, T entity) where T : AuditableEntity;
    Task DeleteAsync<T>(Guid id) where T : AuditableEntity;
    Task PublishSqsMessageAsync<T>(string queueName, T message) where T : IMessage;

    Task<PagedResult<Workday>> GetAllWorkdaysPagedAsync(int pageNumber, int pageSize,
        string? sortByDate, DateOnly? filterByDate, Guid? filterByEmployee);
    
    Task<PagedResult<Department>> GetAllDepartmentsPagedAsync(int pageNumber, int pageSize, string? sortByName, string? filterByName);
    Task<PagedResult<Employee>> GetAllEmployeesPagedAsync(int pageNumber, int pageSize, string? sortByName, string? filterByFirstName, string? filterByLastName, Guid? filterByDepartment, Guid? filterByProject);
    Task<PagedResult<Invoice>> GetAllInvoicesPagedAsync(int pageNumber, int pageSize, string? sortByDate, Guid? filterByEmployee, int? filterByMonth, int? filterByYear);
    Task<PagedResult<Project>> GetAllProjectsPagedAsync(int pageNumber, int pageSize, string? sortByName, string? filterByName);

}