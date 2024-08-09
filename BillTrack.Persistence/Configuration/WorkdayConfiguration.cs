using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillTrack.Persistence.Configuration;

public class WorkdayConfiguration : BaseConfiguration<Workday>
{
    public override void Configure(EntityTypeBuilder<Workday> builder)
    {
        base.Configure(builder);
        
        builder.Property(w => w.Date)
            .IsRequired();

        builder.Property(w => w.Hours)
            .IsRequired();

        builder.Property(w => w.EmployeeId)
            .IsRequired();
    }
}