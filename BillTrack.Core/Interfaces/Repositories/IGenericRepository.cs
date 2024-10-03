using System.Linq.Expressions;
using BillTrack.Domain.Entities;

namespace BillTrack.Core.Interfaces.Repositories;

public interface IGenericRepository<TEntity> where TEntity : AuditableEntity
{
    ValueTask<TEntity?> GetByIdAsync(Guid id);
    Task<TEntity?> GetByIdAsync(Guid id, params Expression<Func<TEntity, object>>[] includes);
    IQueryable<TEntity> GetAllAsync(params Expression<Func<TEntity, object>>[]? includeProperties);
    Task<TEntity> AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate);

}