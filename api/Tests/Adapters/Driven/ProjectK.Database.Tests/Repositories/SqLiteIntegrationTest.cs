using AutoFixture;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectK.Core.Commands.v1.Users;
using ProjectK.Core.Entities;
using ProjectK.Core.Infrastructure.RequestContext;
using ProjectK.Database.Contexts;
using ProjectK.Database.Interceptors;
using ProjectK.Infrastructure.RequestContext;
using ProjectK.Tests.Shared;

namespace ProjectK.Database.Tests.Repositories;

public abstract class SqLiteIntegrationTest
{
    private readonly User _defaultUser = User.CreateInstance(CustomAutoFixture.Create().Create<CreateUserCommand>());

    private readonly DbContextOptions<AppDbContext> _options;
    private readonly IUserContext _userContext;

    protected SqLiteIntegrationTest()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "ProjectK.Api:PaginationCountLimit", 100.ToString() }
            }!)
            .Build();
        var serviceProvider = new ServiceCollection()
            .AddScoped<IUserContext, UserContext>()
            .AddScoped<QueryLimitCommandInterceptor>()
            .AddScoped<ApplyUserInterceptor>()
            .AddScoped<ApplyAuditInfoInterceptor>()
            .AddSingleton<IConfiguration>(configuration)
            .BuildServiceProvider();

        _userContext = serviceProvider.GetRequiredService<IUserContext>();

        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .AddInterceptors(
                serviceProvider.GetRequiredService<QueryLimitCommandInterceptor>(),
                serviceProvider.GetRequiredService<ApplyUserInterceptor>(),
                serviceProvider.GetRequiredService<ApplyAuditInfoInterceptor>()
            )
            .Options;

        using var context = GetContext(_options, _userContext);
        context.Database.EnsureCreated();
        context.Add(_defaultUser);
        context.SaveChanges();

        _userContext.SetUserId(_defaultUser.Id);
    }

    private static AppDbContext GetContext(DbContextOptions<AppDbContext> options, IUserContext userContext)
    {
        var context = new TestAppDbContext(options, userContext);
        return context;
    }

    protected async Task<T> ExecuteCommand<T>(Func<AppDbContext, Task<T>> action)
    {
        await using var context = GetContext(_options, _userContext);
        var act = await action(context);
        await context.SaveChangesAsync();
        return act;
    }

    protected async Task ExecuteCommand(Func<AppDbContext, Task> action)
    {
        await using var context = GetContext(_options, _userContext);
        await action(context);
        await context.SaveChangesAsync();
    }

    protected async Task AddEntities(params object[] entities)
    {
        await using var context = GetContext(_options, _userContext);
        foreach (var item in entities) await context.AddAsync(item);
        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}