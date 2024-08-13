namespace BillTrack.Core.Interfaces.Services;

public interface IGenericService<TResponse, in TRequest>
{
    Task<TResponse?> GetByIdAsync(Guid id);
    IQueryable<TResponse> GetAllAsync();
    Task<TResponse> AddAsync(TRequest entity);
    Task UpdateAsync(Guid id, TRequest entity);
    Task DeleteAsync(Guid id);
}