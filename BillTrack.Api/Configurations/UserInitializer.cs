using BillTrack.Domain.Entities;
using BillTrack.Persistence;

namespace BillTrack.Api.Configurations;

public static class UserInitializer
{
    public static WebApplication Seed(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            context.Database.EnsureCreated();

            var users = context.Users.FirstOrDefault();

            if (users == null)
            {
                context.Users.AddRange(
                    new User
                    {
                        Email = "default_user@email.com", 
                        Password = "9EBA5B4C48DDD855168F207CBFA2D1C9;69FBADCCAC9D53FAE8106E8A371904ABFC7B11E87F2FD3B4AA22F985B0E7A6BF"
                    }
                );

                context.SaveChanges();
            }
        }
        catch
        {
            throw;
        }

        return app;
    }
}