using Mediator;
using ProjectK.Api.UseCases.v1.Transactions.Interfaces;
using ProjectK.Core.Commands.v1.Transactions;

namespace ProjectK.Api.UseCases.v1.Transactions;

internal class DeleteTransactionUseCase(ISender mediator) : IDeleteTransactionUseCase
{
    public async Task ExecuteAsync(Ulid transactionId, CancellationToken cancellationToken = default)
    {
        var command = new DeleteTransactionCommand(transactionId);

        await mediator.Send(command, cancellationToken);
    }
}