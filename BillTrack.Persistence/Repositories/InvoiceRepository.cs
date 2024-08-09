using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillTrack.Persistence.Repositories;

public class InvoiceRepository : GenericRepository<Invoice>
{
    public InvoiceRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}