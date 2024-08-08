namespace BillTrack.Persistence.Repositories;

interface ICrudRepository<T>
{
    ValueTask<T?> GetByIdAsync(Guid id);
    Task<List<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}