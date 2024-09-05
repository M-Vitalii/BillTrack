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
                        Password = "r9uYP5zrRh6IUAzzMvOpEyCyN8h2mAw4luVNv5tJYlE="
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