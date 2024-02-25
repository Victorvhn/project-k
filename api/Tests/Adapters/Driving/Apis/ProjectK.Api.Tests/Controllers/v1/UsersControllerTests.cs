using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ProjectK.Api.Controllers.v1;
using ProjectK.Api.Dtos.v1.Users.Requests;
using ProjectK.Api.Dtos.v1.Users.Responses;
using ProjectK.Api.UseCases.v1.Users.Interfaces;
using ProjectK.Tests.Shared;

namespace ProjectK.Api.Tests.Controllers.v1;

public class UsersControllerTests
{
    private readonly ICreateUserUseCase _createUserUseCase;
    private readonly IDeleteUserUseCase _deleteUserUseCase;
    private readonly Fixture _fixture = CustomAutoFixture.Create();
    private readonly IGetUserByEmailUseCase _getUserByEmailUseCase;
    private readonly IUpdateUserCurrencyUseCase _updateUserCurrencyUseCase;
    
    private readonly UsersController _usersController;

    public UsersControllerTests()
    {
        _createUserUseCase = Substitute.For<ICreateUserUseCase>();
        _getUserByEmailUseCase = Substitute.For<IGetUserByEmailUseCase>();
        _deleteUserUseCase = Substitute.For<IDeleteUserUseCase>();
        _updateUserCurrencyUseCase = Substitute.For<IUpdateUserCurrencyUseCase>();

        _usersController = new UsersController(_createUserUseCase, _getUserByEmailUseCase, _deleteUserUseCase, _updateUserCurrencyUseCase);
    }

    [Fact]
    public async Task Create_Should_Return_Created()
    {
        var request = _fixture.Create<CreateUserRequest>();
        var cancellationToken = new CancellationToken();
        var useCaseResult = _fixture.Create<UserDto>();

        _createUserUseCase
            .ExecuteAsync(request, cancellationToken)
            .Returns(useCaseResult);

        var actionResult = await _usersController.Create(request, cancellationToken);

        var result = actionResult.Result as CreatedResult;
        result
            .Should()
            .NotBeNull();
        result!
            .Value
            .Should()
            .BeEquivalentTo(useCaseResult);
    }

    [Fact]
    public async Task Exists_Should_Return_Ok()
    {
        const string request = "test@email.com";
        var cancellationToken = new CancellationToken();
        var useCaseResult = _fixture.Create<UserDto>();

        _getUserByEmailUseCase
            .ExecuteAsync(request, cancellationToken)
            .Returns(useCaseResult);

        var actionResult = await _usersController.Exists(request, cancellationToken);

        var result = actionResult.Result as OkObjectResult;
        result
            .Should()
            .NotBeNull();
        result!
            .Value
            .Should()
            .BeEquivalentTo(useCaseResult);
    }

    [Fact]
    public async Task Delete_Should_Return_NoContent()
    {
        var userId = _fixture.Create<Ulid>();
        var cancellationToken = new CancellationToken();

        var actionResult = await _usersController.Delete(userId, cancellationToken);

        var result = actionResult as NoContentResult;
        result
            .Should()
            .NotBeNull();
        await _deleteUserUseCase
            .Received(1)
            .ExecuteAsync(userId, cancellationToken);
    }
    
    [Fact]
    public async Task UpdateCurrency_Should_Return_Ok()
    {
        var userId = Ulid.NewUlid();
        var request = _fixture.Create<UpdateCurrencyRequest>();
        var cancellationToken = new CancellationToken();
        var useCaseResult = _fixture.Create<UserDto>();

        _updateUserCurrencyUseCase
            .ExecuteAsync(userId, request, cancellationToken)
            .Returns(useCaseResult);

        var actionResult = await _usersController.UpdateCurrency(userId, request, cancellationToken);

        var result = actionResult.Result as OkObjectResult;
        result
            .Should()
            .NotBeNull();
        result!
            .Value
            .Should()
            .BeEquivalentTo(useCaseResult);
    }
}