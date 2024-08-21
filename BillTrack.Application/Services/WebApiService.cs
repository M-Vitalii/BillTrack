using BillTrack.Core.Exceptions;
using BillTrack.Core.Interfaces.Repositories;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;

namespace BillTrack.Application.Services;

public class WebApiService : IWebApiService
{
    private readonly Dictionary<Type, object> _repositories;

    public WebApiService(
        IGenericRepository<Project> projectRepository,
        IGenericRepository<Department> departmentRepository,
        IGenericRepository<Invoice> invoiceRepository,
        IGenericRepository<Workday> workdayRepository,
        IGenericRepository<Employee> employeeRepository)
    {
        _repositories = new Dictionary<Type, object>
        {
            { typeof(Project), projectRepository },
            { typeof(Department), departmentRepository },
            { typeof(Invoice), invoiceRepository },
            { typeof(Workday), workdayRepository },
            { typeof(Employee), employeeRepository },
        };
    }
    
    private IGenericRepository<T> GetRepository<T>() where T : AuditableEntity
    {
        if (_repositories.TryGetValue(typeof(T), out var repository))
        { 
            return (IGenericRepository<T>)repository;
        }

        throw new InvalidOperationException($"Repository for type {typeof(T)} not found.");
    }
    
    public async Task<T> CreateAsync<T>(T entity) where T : AuditableEntity
    {
        return await GetRepository<T>().AddAsync(entity);
    }

    public async Task<T> GetByIdAsync<T>(Guid id) where T : AuditableEntity
    {
        return await GetByIdAsyncOrThrow(GetRepository<T>(), id);
    }

    public IQueryable<T> GetAll<T>() where T : AuditableEntity
    {
        return GetRepository<T>().GetAllAsync();
    }

    public async Task UpdateAsync<T>(Guid id, T entity) where T : AuditableEntity
    {
        await GetRepository<T>().UpdateAsync(entity);
    }

    public async Task DeleteAsync<T>(Guid id) where T : AuditableEntity
    {
        IGenericRepository<T> repository = GetRepository<T>();
    
        await repository.DeleteAsync(await GetByIdAsyncOrThrow(repository, id));
    }

    private async Task<T> GetByIdAsyncOrThrow<T>(IGenericRepository<T> repository, Guid id) where T : AuditableEntity
    {
        var entity = await repository.GetByIdAsync(id);
        
        if (entity == null)
        {
            throw new NotFoundException($"Entity of type {typeof(T)} with ID {id} not found.");
        }

        return entity;
    }
}