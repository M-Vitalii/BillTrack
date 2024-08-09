using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BillTrack.Persistence.Interceptors;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (eventData.Context is null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        foreach (EntityEntry? entry in eventData.Context.ChangeTracker.Entries())
        {
            switch (entry)
            {
                case { State: EntityState.Deleted, Entity: AuditableEntity delete }:
                    entry.State = EntityState.Modified;
                    delete.IsDeleted = true;
                    delete.DeletedAt = DateTime.UtcNow;
                    break;
                case { State: EntityState.Modified, Entity: AuditableEntity { IsDeleted: true } update }:
                    update.IsDeleted = false;
                    update.DeletedAt = null;
                    break;
            }
        }
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}