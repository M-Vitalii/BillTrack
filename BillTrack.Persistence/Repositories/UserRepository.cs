using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillTrack.Persistence.Repositories;

public class UserRepository : ICrudRepository<User>
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ValueTask<User?> GetByIdAsync(Guid id) => _dbContext.Users.FindAsync(id);

    public Task<List<User>> GetAllAsync() => _dbContext.Users.ToListAsync();

    public async Task<User> AddAsync(User entity)
    {
        await _dbContext.Users.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task UpdateAsync(User entity)
    {
        _dbContext.Users.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(User entity)
    {
        _dbContext.Users.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}