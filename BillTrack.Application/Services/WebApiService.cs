using System.Linq.Expressions;
using BillTrack.Application.Filters;
using BillTrack.Core.Exceptions;
using BillTrack.Core.Interfaces.Models;
using BillTrack.Core.Interfaces.Repositories;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Core.Models;
using BillTrack.Core.Models.WebApi;
using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BillTrack.Application.Services;

public class WebApiService : IWebApiService
{
    private IServiceScope _scope;
    private readonly ISqsPublisher _sqsPublisher;

    public WebApiService(IServiceScopeFactory serviceScopeFactory, ISqsPublisher sqsPublisher)
    {
        _scope = serviceScopeFactory.CreateScope();
        _sqsPublisher = sqsPublisher;
    }

    private IGenericRepository<T> GetRepository<T>() where T : AuditableEntity
    {
        return this._scope.ServiceProvider.GetRequiredService<IGenericRepository<T>>();
    }

    public async Task<T> CreateAsync<T>(T entity) where T : AuditableEntity
    {
        return await GetRepository<T>().AddAsync(entity);
    }

    public async Task<T> GetByIdAsync<T>(Guid id) where T : AuditableEntity
    {
        return await GetByIdAsyncOrThrow(GetRepository<T>(), id);
    }

    public async Task<PagedResult<T>> GetAllPagedAsync<T>(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Expression<Func<T, object>>[]? includes = null,
        string? sortDirection = SortDirection.Asc) where T : AuditableEntity
    {
        var items = await GetRepository<T>().GetAllAsync(filter, orderBy, includes)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<T>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
        };
    }
    
    public async Task UpdateAsync<T>(Guid id, T entity) where T : AuditableEntity
    {
        await GetRepository<T>().UpdateAsync(entity);
    }

    public async Task DeleteAsync<T>(Guid id) where T : AuditableEntity
    {
        var repository = GetRepository<T>();

        var entity = await GetByIdAsyncOrThrow(repository, id);

        await repository.DeleteAsync(entity);
    }

    private async Task<T> GetByIdAsyncOrThrow<T>(IGenericRepository<T> repository, Guid id) where T : AuditableEntity
    {
        var entity = await repository.GetByIdAsync(id);

        if (entity == null)
        {
            throw new NotFoundException($"Entity with ID {id} not found.");
        }

        return entity;
    }

    public async Task PublishSqsMessageAsync<T>(string queueName, T message) where T : IMessage
    {
        await _sqsPublisher.PublishMessageAsync(queueName, message);
    }

    public Task<PagedResult<Workday>> GetAllWorkdaysPagedAsync(
        int pageNumber,
        int pageSize,
        string? sortByDate,
        DateOnly? filterByDate,
        Guid? filterByEmployee)
    {
        var filter = EntityFilter.BuildWorkdayFilter(filterByDate, filterByEmployee);
        var orderBy = CreateOrderByFunc<Workday, DateOnly>(sortByDate, w => w.Date);

        Expression<Func<Workday, object>>[] includes = { w => w.Employee }; 
        
        return GetAllPagedAsync(pageNumber, pageSize, filter, orderBy, includes);
    }

    public Task<PagedResult<Employee>> GetAllEmployeesPagedAsync(
        int pageNumber,
        int pageSize,
        string? sortByName,
        string? filterByFullname,
        Guid? filterByDepartment,
        Guid? filterByProject)
    {
        var filter =
            EntityFilter.BuildEmployeeFilter(filterByFullname, filterByDepartment, filterByProject);
        var orderBy = CreateOrderByFunc<Employee, string>(sortByName, e => e.Lastname);

        Expression<Func<Employee, object>>[] includes = 
        { 
            e => e.Department, 
            e => e.Project
        };
        
        return GetAllPagedAsync(pageNumber, pageSize, filter, orderBy, includes);
    }

    public Task<PagedResult<Department>> GetAllDepartmentsPagedAsync(
        int pageNumber,
        int pageSize,
        string? sortByName,
        string? filterByName)
    {
        var filter = EntityFilter.BuildDepartmentFilter(filterByName);
        var orderBy = CreateOrderByFunc<Department, string>(sortByName, d => d.Name);

        return GetAllPagedAsync(pageNumber, pageSize, filter, orderBy);
    }

    public Task<PagedResult<Project>> GetAllProjectsPagedAsync(
        int pageNumber,
        int pageSize,
        string? sortByName,
        string? filterByName)
    {
        var filter = EntityFilter.BuildProjectFilter(filterByName);
        var orderBy = CreateOrderByFunc<Project, string>(sortByName, p => p.Name);

        return GetAllPagedAsync(pageNumber, pageSize, filter, orderBy);
    }

    public Task<PagedResult<Invoice>> GetAllInvoicesPagedAsync(
        int pageNumber,
        int pageSize,
        string? sortByDate,
        Guid? filterByEmployee,
        int? filterByMonth,
        int? filterByYear)
    {
        var filter = EntityFilter.BuildInvoiceFilter(filterByEmployee, filterByMonth, filterByYear);
        var orderBy = CreateOrderByFunc<Invoice, int>(sortByDate, i => i.Year * 100 + i.Month);
        
        Expression<Func<Invoice, object>>[] includes = { i => i.Employee }; 

        return GetAllPagedAsync(pageNumber, pageSize, filter, orderBy, includes);
    }

    private Func<IQueryable<T>, IOrderedQueryable<T>>? CreateOrderByFunc<T, TKey>(
        string? sortDirection,
        Expression<Func<T, TKey>> keySelector) where T : AuditableEntity
    {
        if (string.IsNullOrEmpty(sortDirection)) return null;

        return sortDirection.ToLower() switch
        {
            SortDirection.Asc => q => q.OrderBy(keySelector),
            SortDirection.Desc => q => q.OrderByDescending(keySelector),
            _ => q => q.OrderBy(keySelector)
        };
    }
}