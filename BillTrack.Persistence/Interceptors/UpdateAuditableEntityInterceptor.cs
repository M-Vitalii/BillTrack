using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BillTrack.Persistence.Interceptors;

public class UpdateAuditableEntityInterceptor : SaveChangesInterceptor
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
                case { State: EntityState.Added, Entity: AuditableEntity add }:
                    add.CreatedAt = DateTime.UtcNow;
                    add.UpdatedAt = DateTime.UtcNow;
                    break;
                case { State: EntityState.Modified, Entity: AuditableEntity update }:
                    update.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}