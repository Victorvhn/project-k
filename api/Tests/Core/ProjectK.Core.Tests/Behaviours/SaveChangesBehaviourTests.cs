using Mediator;
using NSubstitute;
using ProjectK.Core.Adapters.Driven.Database;
using ProjectK.Core.Behaviours;

namespace ProjectK.Core.Tests.Behaviours;

public class UnitOfWorkBehaviorWithResponseTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    [Fact]
    public async Task Handle_should_save_changes_when_request_is_a_command()
    {
        var behavior = new SaveChangesBehaviour<TestCommand, Unit>(_unitOfWork);

        await behavior.Handle(new TestCommand("test"), CancellationToken.None,
            Substitute.For<MessageHandlerDelegate<TestCommand, Unit>>());

        await _unitOfWork
            .Received(1)
            .SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_should_not_save_changes_when_request_is_not_a_command()
    {
        var behavior = new SaveChangesBehaviour<TestQuery, object>(_unitOfWork);

        await behavior.Handle(new TestQuery("test"), CancellationToken.None,
            Substitute.For<MessageHandlerDelegate<TestQuery, object>>());

        await _unitOfWork
            .DidNotReceiveWithAnyArgs()
            .SaveChangesAsync();
    }

    private record TestCommand(string Test) : ICommand;

    private record TestQuery(string Test) : IRequest<object>;
}