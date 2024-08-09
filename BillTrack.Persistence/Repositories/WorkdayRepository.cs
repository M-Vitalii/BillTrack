using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillTrack.Persistence.Repositories;

public class WorkdayRepository : GenericRepository<Workday>
{
    public WorkdayRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}