namespace BillTrack.Persistence.Repositories;

interface IGenericRepository<T>
{
    ValueTask<T?> GetByIdAsync(Guid id);
    IQueryable<T> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}