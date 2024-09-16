using BillTrack.Core.Models;
using BillTrack.Domain.Entities;

namespace BillTrack.Core.Interfaces.Services;

public interface IWebApiService
{
    Task<T> CreateAsync<T>(T entity) where T : AuditableEntity;
    Task<T> GetByIdAsync<T>(Guid id) where T : AuditableEntity;
    Task<PagedResult<T>> GetAllPagedAsync<T>(int pageNumber, int pageSize) where T : AuditableEntity;
    Task UpdateAsync<T>(Guid id, T entity) where T : AuditableEntity;
    Task DeleteAsync<T>(Guid id) where T : AuditableEntity;
    Task PublishSqsMessageAsync<T>(T message);
}