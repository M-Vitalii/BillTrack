using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillTrack.Persistence.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.Password)
            .HasMaxLength(100)
            .IsRequired();
    }
}