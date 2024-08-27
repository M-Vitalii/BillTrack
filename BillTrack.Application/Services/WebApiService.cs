using BillTrack.Core.Exceptions;
using BillTrack.Core.Interfaces.Repositories;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace BillTrack.Application.Services;

public class WebApiService : IWebApiService
{
    private IServiceScope _scope;

    public WebApiService(IServiceScopeFactory serviceScopeFactory)
    {
        _scope = serviceScopeFactory.CreateScope();
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