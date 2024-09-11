using BillTrack.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BillTrack.Api.Configurations;

public static class MigrationsConfiguration
{
    public static async Task UseDatabaseMigrations(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        await using var scope = app.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var pendingMigrations = await db.Database.GetPendingMigrationsAsync();
        
        if (pendingMigrations.Any())
        {
            await db.Database.MigrateAsync();
        }
    }
}