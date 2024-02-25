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

public class UpdateUserCurrencyUseCaseTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly IUpdateUserCurrencyUseCase _createUserUseCase;

    private readonly ISender _sender;

    public UpdateUserCurrencyUseCaseTests()
    {
        var mapper = MapperFixture.GetMapper();
        _sender = Substitute.For<ISender>();

        _createUserUseCase = new UpdateUserCurrencyUseCase(mapper, _sender);
    }

    [Fact]
    public async Task It_should_patch_the_user()
    {
        // Arrange
        var userId = Ulid.NewUlid();
        var request = Fixture.Create<UpdateCurrencyRequest>();
        var user = Fixture.Create<User>();

        _sender
            .Send(Arg.Is<UpdateUserCurrencyCommand>(a =>
                a.UserId == userId &&
                a.Currency == request.Currency
            ))
            .Returns(user);

        // Act
        var result = await _createUserUseCase.ExecuteAsync(userId, request);

        // Assert
        result
            .Should()
            .BeEquivalentTo(user, opt => opt.ExcludingMissingMembers());
    }
}