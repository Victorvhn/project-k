using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectK.Core.Entities;

namespace ProjectK.Database.Mappings;

[ExcludeFromCodeCoverage]
public class PlannedTransactionMapping : IEntityTypeConfiguration<PlannedTransaction>
{
    public void Configure(EntityTypeBuilder<PlannedTransaction> builder)
    {
        builder
            .ToTable("planned_transactions");

        builder
            .HasKey(h => h.Id);

        builder
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(p => p.Description)
            .HasMaxLength(PlannedTransaction.Constraints.DescriptionMaxLength)
            .IsRequired();

        builder
            .Property(p => p.Amount)
            .HasPrecision(18, 4)
            .IsRequired();

        builder
            .Property(p => p.AmountType)
            .IsRequired();

        builder
            .Property(p => p.Type)
            .IsRequired();

        builder
            .Property(p => p.Recurrence)
            .IsRequired();

        builder
            .Property(p => p.StartsAt)
            .IsRequired();

        builder
            .Property(p => p.EndsAt)
            .IsRequired(false);

        builder
            .HasOne(h => h.Category)
            .WithMany(w => w.PlannedTransactions)
            .HasForeignKey(h => h.CategoryId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        builder
            .HasOne(h => h.User)
            .WithMany(w => w.PlannedTransactions)
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