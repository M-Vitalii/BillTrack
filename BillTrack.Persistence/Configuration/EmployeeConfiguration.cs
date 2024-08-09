using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillTrack.Persistence.Configuration;

public class EmployeeConfiguration : BaseConfiguration<Employee>
{
    public override void Configure(EntityTypeBuilder<Employee> builder)
    {
        base.Configure(builder);
            
        builder.Property(e => e.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Firstname)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Lastname)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Salary)
            .IsRequired();

        builder.Property(e => e.DepartmentId)
            .IsRequired();

        builder.Property(e => e.ProjectId)
            .IsRequired();

        builder.HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId);

        builder.HasOne(e => e.Project)
            .WithMany(p => p.Employees)
            .HasForeignKey(e => e.ProjectId);

        builder.HasMany(e => e.Workdays)
            .WithOne(w => w.Employee)
            .HasForeignKey(w => w.EmployeeId);

        builder.HasMany(e => e.Invoices)
            .WithOne(i => i.Employee)
            .HasForeignKey(i => i.EmployeeId);
    }
}