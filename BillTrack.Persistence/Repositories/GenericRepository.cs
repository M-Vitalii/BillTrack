using System.Linq.Expressions;
using BillTrack.Core.Interfaces.Repositories;
using BillTrack.Core.Models;
using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillTrack.Persistence.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : AuditableEntity
{
    private readonly AppDbContext _dbContext;

    public GenericRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ValueTask<TEntity?> GetByIdAsync(Guid id) => _dbContext.Set<TEntity>().FindAsync(id);
    
    public IQueryable<TEntity> GetAllAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        params Expression<Func<TEntity, object>>[]? includes)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>().AsNoTracking();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return query;
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
    }
    
    public async Task<TEntity?> GetByIdAsync(Guid id, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(e => e.Id == id);
    }
}