using Mediator;
using NSubstitute;
using ProjectK.Api.UseCases.v1.Users;
using ProjectK.Core.Commands.v1.Users;

namespace ProjectK.Api.Tests.UseCases.v1.Users;

public class DeleteUserUseCaseTests
{
    private readonly DeleteUserUseCase _deleteUserUseCase;
    private readonly ISender _sender;

    public DeleteUserUseCaseTests()
    {
        _sender = Substitute.For<ISender>();

        _deleteUserUseCase = new DeleteUserUseCase(_sender);
    }

    [Fact]
    public async Task It_should_delete()
    {
        // Arrange
        var userId = Ulid.NewUlid();

        // Act
        await _deleteUserUseCase.ExecuteAsync(userId);

        // Assert
        await _sender
            .Received(1)
            .Send(Arg.Is<DeleteUserCommand>(a => a.UserId == userId),
                Arg.Any<CancellationToken>()
            );
    }
}