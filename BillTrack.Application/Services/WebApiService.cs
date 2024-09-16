using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using BillTrack.Core.Exceptions;
using BillTrack.Core.Interfaces.Repositories;
using BillTrack.Core.Interfaces.Services;
using BillTrack.Core.Models;
using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
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

    public async Task<PagedResult<T>> GetAllPagedAsync<T>(int pageNumber, int pageSize) where T : AuditableEntity
    {
        var items = await GetRepository<T>().GetAllAsync()
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
            throw new NotFoundException($"Entity of type {typeof(T)} with ID {id} not found.");
        }

        return entity;
    }
    
    public async Task PublishSqsMessageAsync<T>(T message)
    {
        await _sqsPublisher.PublishMessageAsync(message);
    }
}