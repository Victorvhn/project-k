using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectK.Core.Entities;

namespace ProjectK.Database.Mappings;

public class CustomPlannedTransactionMapping : IEntityTypeConfiguration<CustomPlannedTransaction>
{
    public void Configure(EntityTypeBuilder<CustomPlannedTransaction> builder)
    {
        builder
            .ToTable("custom_planned_transactions");

        builder
            .HasKey(h => h.Id);

        builder
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(p => p.Description)
            .HasMaxLength(CustomPlannedTransaction.Constraints.DescriptionMaxLength)
            .IsRequired();

        builder
            .Property(p => p.Amount)
            .HasPrecision(18, 4)
            .IsRequired();

        builder
            .Property(p => p.RefersTo)
            .IsRequired();

        builder
            .Property(p => p.Active)
            .HasDefaultValue(true)
            .IsRequired();

        builder
            .HasOne(h => h.BasePlannedTransaction)
            .WithMany(w => w.CustomPlannedTransactions)
            .HasForeignKey(h => h.BasePlannedTransactionId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder
            .HasOne(h => h.User)
            .WithMany(w => w.CustomPlannedTransactions)
            .HasForeignKey(h => h.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        builder
            .Property(p => p.CreatedAtUtc)
            .IsRequired();
        builder
            .Property(p => p.CreatedBy)
            .IsRequired();
        builder
            .Property(p => p.UpdatedAtUtc)
            .IsRequired(false);
        builder
            .Property(p => p.UpdatedBy)
            .IsRequired(false);
    }
}