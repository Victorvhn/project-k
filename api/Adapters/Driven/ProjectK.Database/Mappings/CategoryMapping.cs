using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectK.Core.Entities;

namespace ProjectK.Database.Mappings;

public class CategoryMapping : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .ToTable("categories");

        builder
            .HasKey(h => h.Id);

        builder
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(p => p.Name)
            .HasMaxLength(Category.Constraints.NameMaxLength)
            .IsRequired();

        builder
            .Property(p => p.HexColor)
            .HasDefaultValue(Category.DefaultColor)
            .IsRequired();

        builder
            .HasOne(h => h.User)
            .WithMany(w => w.Categories)
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