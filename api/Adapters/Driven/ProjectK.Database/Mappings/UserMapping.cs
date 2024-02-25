using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectK.Core.Entities;
using ProjectK.Core.Enums;

namespace ProjectK.Database.Mappings;

[ExcludeFromCodeCoverage]
public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .ToTable("users");

        builder
            .HasKey(h => h.Id);

        builder
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(p => p.Name)
            .HasMaxLength(User.Constraints.NameMaxLength)
            .IsRequired();

        builder
            .Property(p => p.Email)
            .HasMaxLength(User.Constraints.EmailMaxLength)
            .IsRequired();
        
        builder
            .Property(p => p.Currency)
            .IsRequired()
            .HasDefaultValue(Currency.Brl);

        builder
            .HasIndex(h => h.Email)
            .IsUnique();
    }
}