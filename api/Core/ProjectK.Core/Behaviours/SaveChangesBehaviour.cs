using Mediator;
using ProjectK.Core.Adapters.Driven.Database;

namespace ProjectK.Core.Behaviours;

public sealed class SaveChangesBehaviour<TMessage, TResponse>(IUnitOfWork unitOfWork)
    : MessagePostProcessor<TMessage, TResponse>
    where TMessage : IMessage
{
    protected override async ValueTask Handle(TMessage message, TResponse response, CancellationToken cancellationToken)
    {
        if (!IsCommand()) return;

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static bool IsCommand()
    {
        return typeof(IBaseCommand).IsAssignableFrom(typeof(TMessage));
    }
}