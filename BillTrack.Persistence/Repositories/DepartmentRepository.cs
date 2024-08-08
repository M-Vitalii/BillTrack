using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillTrack.Persistence.Repositories;

public class DepartmentRepository : ICrudRepository<Department>
{
    private readonly AppDbContext _dbContext;

    public DepartmentRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ValueTask<Department?> GetByIdAsync(Guid id) => _dbContext.Departments.FindAsync(id);

    public Task<List<Department>> GetAllAsync() => _dbContext.Departments.ToListAsync();

    public async Task<Department> AddAsync(Department entity)
    {
        await _dbContext.Departments.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task UpdateAsync(Department entity)
    {
        _dbContext.Departments.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Department entity)
    {
        _dbContext.Departments.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}