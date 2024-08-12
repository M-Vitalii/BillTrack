using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillTrack.Persistence.Configuration;

public class ProjectConfiguration : BaseConfiguration<Project>
{
    public override void Configure(EntityTypeBuilder<Project> builder)
    {
        base.Configure(builder);
        
        builder.Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasMany(p => p.Employees)
            .WithOne(e => e.Project)
            .HasForeignKey(e => e.ProjectId);
    }
}