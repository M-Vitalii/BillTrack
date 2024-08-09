using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillTrack.Persistence.Repositories;

public class ProjectRepository : GenericRepository<Project>
{
    public ProjectRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}