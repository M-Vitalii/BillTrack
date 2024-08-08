using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillTrack.Persistence.Repositories;

public class ProjectRepository : ICrudRepository<Project>
{
    private readonly AppDbContext _dbContext;

    public ProjectRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ValueTask<Project?> GetByIdAsync(Guid id) => _dbContext.Projects.FindAsync(id);

    public Task<List<Project>> GetAllAsync() => _dbContext.Projects.ToListAsync();

    public async Task<Project> AddAsync(Project entity)
    {
        await _dbContext.Projects.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task UpdateAsync(Project entity)
    {
        _dbContext.Projects.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Project entity)
    {
        _dbContext.Projects.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}