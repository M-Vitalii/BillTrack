using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillTrack.Persistence.Configuration;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
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
        
        builder.HasIndex(e => e.DepartmentId)
            .IsUnique();

        builder.HasIndex(e => e.ProjectId)
            .IsUnique();
        
        builder.HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Project)
            .WithMany(p => p.Employees)
            .HasForeignKey(e => e.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Workdays)
            .WithOne(w => w.Employee)
            .HasForeignKey(w => w.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Invoices)
            .WithOne(i => i.Employee)
            .HasForeignKey(i => i.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}