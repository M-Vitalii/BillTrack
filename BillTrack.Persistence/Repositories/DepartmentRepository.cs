using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillTrack.Persistence.Repositories;

public class DepartmentRepository : GenericRepository<Department>
{
    public DepartmentRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}