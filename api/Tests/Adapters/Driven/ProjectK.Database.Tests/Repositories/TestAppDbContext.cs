using Microsoft.EntityFrameworkCore;
using ProjectK.Core.Infrastructure.RequestContext;
using ProjectK.Database.Contexts;

namespace ProjectK.Database.Tests.Repositories;

public class TestAppDbContext(
    DbContextOptions<AppDbContext> options,
    IUserContext userContext)
    : AppDbContext(options, userContext)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        FixIntegrationTests(modelBuilder);
    }

    private static void FixIntegrationTests(ModelBuilder modelBuilder)
    {
        foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes())
        {
            var dateTimeProperties = mutableEntityType.ClrType.GetProperties().Where(w =>
                w.PropertyType == typeof(decimal) || w.PropertyType == typeof(decimal?));

            foreach (var property in dateTimeProperties)
                modelBuilder
                    .Entity(mutableEntityType.Name)
                    .Property(property.Name)
                    .HasConversion<double>();
        }
    }
}