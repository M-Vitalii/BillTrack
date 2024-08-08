using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillTrack.Persistence.Configuration;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.Property(i => i.Month)
            .IsRequired();

        builder.Property(i => i.Year)
            .IsRequired();

        builder.Property(i => i.EmployeeId)
            .IsRequired();

        builder.HasIndex(i => i.EmployeeId)
            .IsUnique();

        builder.HasOne(i => i.Employee)
            .WithMany(e => e.Invoices)
            .HasForeignKey(i => i.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable(it => it.HasCheckConstraint("CK_Invoice_Month", "\"Month\" > 0 AND \"Month\" < 13"));
        builder.ToTable(it => it.HasCheckConstraint("CK_Invoice_Year", "\"Year\" > 0"));
    }
}