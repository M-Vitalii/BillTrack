using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillTrack.Persistence.Configuration;

public class WorkdayConfiguration : IEntityTypeConfiguration<Workday>
{
    public void Configure(EntityTypeBuilder<Workday> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.Date)
            .IsRequired();

        builder.Property(w => w.Hours)
            .IsRequired();

        builder.Property(w => w.EmployeeId)
            .IsRequired();

        builder.HasIndex(w => w.EmployeeId)
            .IsUnique();
    }
}