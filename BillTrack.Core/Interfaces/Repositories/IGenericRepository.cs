using System.Linq.Expressions;
using BillTrack.Domain.Entities;

namespace BillTrack.Core.Interfaces.Repositories;

public interface IGenericRepository<TEntity> where TEntity : AuditableEntity
{
    ValueTask<TEntity?> GetByIdAsync(Guid id);
    IQueryable<TEntity> GetAllAsync();
    Task<TEntity> AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate);

}