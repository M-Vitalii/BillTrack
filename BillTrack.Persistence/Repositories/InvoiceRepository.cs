using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillTrack.Persistence.Repositories;

public class InvoiceRepository : ICrudRepository<Invoice>
{
    private readonly AppDbContext _dbContext;

    public InvoiceRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ValueTask<Invoice?> GetByIdAsync(Guid id) => _dbContext.Invoices.FindAsync(id);

    public Task<List<Invoice>> GetAllAsync() => _dbContext.Invoices.ToListAsync();

    public async Task<Invoice> AddAsync(Invoice entity)
    {
        await _dbContext.Invoices.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        
        return entity;
    }

    public async Task UpdateAsync(Invoice entity)
    {
        _dbContext.Invoices.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Invoice entity)
    {
        _dbContext.Invoices.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}