using AutoFixture;
using FluentAssertions;
using Mediator;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using ProjectK.Api.Dtos.v1.Users.Requests;
using ProjectK.Api.UseCases.v1.Users;
using ProjectK.Api.UseCases.v1.Users.Interfaces;
using ProjectK.Core.Commands.v1.Users;
using ProjectK.Core.Entities;
using ProjectK.Tests.Shared;

namespace ProjectK.Api.Tests.UseCases.v1.Users;

public class CreateUserUseCaseTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly ICreateUserUseCase _createUserUseCase;

    private readonly ISender _sender;

    public CreateUserUseCaseTests()
    {
        var mapper = MapperFixture.GetMapper();
        _sender = Substitute.For<ISender>();

        _createUserUseCase = new CreateUserUseCase(mapper, _sender);
    }

    [Fact]
    public async Task It_should_create_a_user()
    {
        // Arrange
        var request = Fixture.Create<CreateUserRequest>();
        var user = Fixture.Create<User>();

        _sender
            .Send(Arg.Is<CreateUserCommand>(a =>
                a.Name == request.Name &&
                a.Email == request.Email
            ))
            .Returns(user);

        // Act
        var result = await _createUserUseCase.ExecuteAsync(request);

        // Assert
        result
            .Should()
            .BeEquivalentTo(user, opt => opt.ExcludingMissingMembers());
    }

    [Fact]
    public async Task It_should_return_null_when_service_returns_null()
    {
        // Arrange
        var request = Fixture.Create<CreateUserRequest>();

        _sender
            .Send(Arg.Is<CreateUserCommand>(a =>
                a.Name == request.Name &&
                a.Email == request.Email
            ))
            .ReturnsNull();

        // Act
        var result = await _createUserUseCase.ExecuteAsync(request);

        // Assert
        result
            .Should()
            .BeNull();
    }
}