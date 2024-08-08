using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillTrack.Persistence.Repositories;

public class EmployeeRepository : ICrudRepository<Employee>
{
    private readonly AppDbContext _dbContext;

    public EmployeeRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ValueTask<Employee?> GetByIdAsync(Guid id) => _dbContext.Employees.FindAsync(id);

    public Task<List<Employee>> GetAllAsync() => _dbContext.Employees.ToListAsync();

    public async Task<Employee> AddAsync(Employee entity)
    {
        await _dbContext.Employees.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        
        return entity;
    }

    public async Task UpdateAsync(Employee entity)
    {
        _dbContext.Employees.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Employee entity)
    {
        _dbContext.Employees.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}