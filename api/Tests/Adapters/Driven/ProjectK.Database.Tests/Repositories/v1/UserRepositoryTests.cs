using FluentAssertions;
using ProjectK.Database.Repositories.v1;
using ProjectK.Tests.Shared.Builders.Entities;

namespace ProjectK.Database.Tests.Repositories.v1;

public class UserRepositoryTests : SqLiteIntegrationTest
{
    [Fact]
    public async Task ExistsByEmailAsync_should_return_true_when_email_exists()
    {
        var user = UserBuilder.Create();

        await AddEntities(user);

        var result = await ExecuteCommand(async context =>
        {
            var repository = new UserRepository(context);

            return await repository.ExistsByEmailAsync(user.Email);
        });

        result
            .Should()
            .BeTrue();
    }

    [Fact]
    public async Task ExistsByEmailAsync_should_return_false_when_email_does_not_exist()
    {
        var result = await ExecuteCommand(async context =>
        {
            var repository = new UserRepository(context);

            return await repository.ExistsByEmailAsync("teste@email.com");
        });

        result
            .Should()
            .BeFalse();
    }

    [Fact]
    public async Task GetByEmailAsync_should_return_user_when_email_exists()
    {
        var user = UserBuilder.Create();

        await AddEntities(user);

        var result = await ExecuteCommand(async context =>
        {
            var repository = new UserRepository(context);

            return await repository.GetByEmailAsync(user.Email);
        });

        result
            .Should()
            .BeEquivalentTo(user);
    }
}