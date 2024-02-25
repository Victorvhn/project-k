using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProjectK.Core.Entities.Base;
using ProjectK.Core.Infrastructure.RequestContext;
using ProjectK.Database.Converters;
using ProjectK.Database.Mappings;

namespace ProjectK.Database.Contexts;

public class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IUserContext userContext)
    : DbContext(options)
{
    private readonly Ulid? _sessionUserId = userContext.UserId;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMapping());
        modelBuilder.ApplyConfiguration(new CategoryMapping());
        modelBuilder.ApplyConfiguration(new PlannedTransactionMapping());
        modelBuilder.ApplyConfiguration(new CustomPlannedTransactionMapping());
        modelBuilder.ApplyConfiguration(new TransactionMapping());

        AddGlobalUserFilter(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<Ulid>()
            .HaveConversion<UlidToStringConverter>()
            .HaveConversion<UlidToBytesConverter>();
    }

    private void AddGlobalUserFilter(ModelBuilder modelBuilder)
    {
        var userBasedEntities = typeof(IUserBasedEntity).Assembly.GetTypes()
            .Where(type => typeof(IUserBasedEntity)
                               .IsAssignableFrom(type)
                           && type is { IsClass: true, IsAbstract: false });

        foreach (var userBasedEntity in userBasedEntities)
            modelBuilder.Entity(userBasedEntity)
                .HasQueryFilter(
                    GenerateUserIdFilterLambdaExpression(userBasedEntity)
                );
    }

    private LambdaExpression GenerateUserIdFilterLambdaExpression(Type type)
    {
        // h =>
        var parameter = Expression.Parameter(type, "h");

        // h.UserId
        var entityUserId = Expression.Property(parameter, nameof(IUserBasedEntity.UserId));

        // _sessionUserId
        Expression<Func<Ulid>> id = () => _sessionUserId ?? Ulid.Empty;
        var currentUserId = id.Body;

        // h.UserId == _sessionUserId
        var idEqualsExpression = Expression.Equal(entityUserId, currentUserId);

        // h => h.UserId == _sessionUserId
        var lambda = Expression.Lambda(idEqualsExpression, parameter);

        return lambda;
    }
}