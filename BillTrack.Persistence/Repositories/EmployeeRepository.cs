using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillTrack.Persistence.Repositories;

public class EmployeeRepository : GenericRepository<Employee>
{
    public EmployeeRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}