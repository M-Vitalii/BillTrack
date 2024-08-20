using BillTrack.Domain.Entities;

namespace BillTrack.Core.Interfaces.Services;

public interface IWebApiService
{
    Task<T> CreateAsync<T>(T entity) where T : AuditableEntity;
    Task<T?> GetByIdAsync<T>(Guid id) where T : AuditableEntity;
    IQueryable<T> GetAll<T>() where T : AuditableEntity;
    Task UpdateAsync<T>(Guid id, T entity) where T : AuditableEntity;
    Task DeleteAsync<T>(Guid id) where T : AuditableEntity;
}