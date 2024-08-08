using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillTrack.Persistence.Repositories;

public class WorkdayRepository : ICrudRepository<Workday>
{
    private readonly AppDbContext _dbContext;

    public WorkdayRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ValueTask<Workday?> GetByIdAsync(Guid id) => _dbContext.Workdays.FindAsync(id);

    public Task<List<Workday>> GetAllAsync() => _dbContext.Workdays.ToListAsync();

    public async Task<Workday> AddAsync(Workday entity)
    {
        await _dbContext.Workdays.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task UpdateAsync(Workday entity)
    {
        _dbContext.Workdays.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Workday entity)
    {
        _dbContext.Workdays.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}