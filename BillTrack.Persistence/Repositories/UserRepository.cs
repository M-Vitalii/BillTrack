using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillTrack.Persistence.Repositories;

public class UserRepository : GenericRepository<User>
{
    public UserRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}