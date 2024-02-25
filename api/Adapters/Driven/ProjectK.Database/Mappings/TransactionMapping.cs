using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectK.Core.Entities;

namespace ProjectK.Database.Mappings;

public class TransactionMapping : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder
            .ToTable("transactions");

        builder
            .HasKey(h => h.Id);

        builder
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(p => p.Description)
            .HasMaxLength(Transaction.Constraints.DescriptionMaxLength)
            .IsRequired();

        builder
            .Property(p => p.Amount)
            .HasPrecision(18, 4)
            .IsRequired();

        builder
            .Property(p => p.Type)
            .IsRequired();

        builder
            .Property(p => p.PaidAt)
            .IsRequired();

        builder
            .HasOne(h => h.Category)
            .WithMany(w => w.Transactions)
            .HasForeignKey(h => h.CategoryId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        builder
            .HasOne(h => h.PlannedTransaction)
            .WithMany(w => w.Transactions)
            .HasForeignKey(h => h.PlannedTransactionId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        builder
            .HasOne(h => h.User)
            .WithMany(w => w.Transactions)
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