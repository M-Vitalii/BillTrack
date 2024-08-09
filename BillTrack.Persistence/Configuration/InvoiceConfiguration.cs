using BillTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillTrack.Persistence.Configuration;

public class InvoiceConfiguration : BaseConfiguration<Invoice>
{
    public override void Configure(EntityTypeBuilder<Invoice> builder)
    {
        base.Configure(builder);
        
        builder.Property(i => i.Month)
            .IsRequired();

        builder.Property(i => i.Year)
            .IsRequired();

        builder.Property(i => i.EmployeeId)
            .IsRequired();

        builder.HasOne(i => i.Employee)
            .WithMany(e => e.Invoices)
            .HasForeignKey(i => i.EmployeeId);

        builder.ToTable(it => it.HasCheckConstraint("CK_Invoice_Month", "\"Month\" > 0 AND \"Month\" < 13"));
        builder.ToTable(it => it.HasCheckConstraint("CK_Invoice_Year", "\"Year\" > 0"));
    }
}