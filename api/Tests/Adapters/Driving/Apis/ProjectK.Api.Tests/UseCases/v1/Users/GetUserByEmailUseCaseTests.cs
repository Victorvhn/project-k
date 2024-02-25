using FluentAssertions;
using Mediator;
using NSubstitute;
using ProjectK.Api.UseCases.v1.Users;
using ProjectK.Api.UseCases.v1.Users.Interfaces;
using ProjectK.Core.Queries.v1.Users;
using ProjectK.Tests.Shared;
using ProjectK.Tests.Shared.Builders.Entities;

namespace ProjectK.Api.Tests.UseCases.v1.Users;

public class GetUserByEmailUseCaseTests
{
    private readonly IGetUserByEmailUseCase _getUserByEmailUseCase;
    private readonly ISender _sender;

    public GetUserByEmailUseCaseTests()
    {
        var mapper = MapperFixture.GetMapper();
        _sender = Substitute.For<ISender>();

        _getUserByEmailUseCase = new GetUserByEmailUseCase(mapper, _sender);
    }

    [Fact]
    public async Task It_should_get_user()
    {
        // Arrange
        const string userEmail = "email@test.com";
        var user = new UserBuilder()
            .Build();

        _sender
            .Send(Arg.Is<GetUserByEmailQuery>(a =>
                a.Email == userEmail))
            .Returns(user);

        // Act
        var result = await _getUserByEmailUseCase.ExecuteAsync(userEmail);

        // Assert
        result
            .Should()
            .BeEquivalentTo(user, opt => opt.ExcludingMissingMembers());
    }
}