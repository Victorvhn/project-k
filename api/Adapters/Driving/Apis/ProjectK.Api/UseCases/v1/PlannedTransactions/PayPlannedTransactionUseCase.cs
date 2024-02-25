using Mediator;
using ProjectK.Api.Dtos.v1.Monthly.Requests;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Requests;
using ProjectK.Api.UseCases.v1.PlannedTransactions.Interfaces;
using ProjectK.Core.Commands.v1.PlannedTransactions;

namespace ProjectK.Api.UseCases.v1.PlannedTransactions;

internal class PayPlannedTransactionUseCase(
    ISender mediator) : IPayPlannedTransactionUseCase
{
    public async Task ExecuteAsync(Ulid plannedTransactionId, MonthlyRequest monthlyRequest,
        PayPlannedTransactionRequest request, CancellationToken cancellationToken = default)
    {
        var command = new PayPlannedTransactionCommand(
            plannedTransactionId,
            monthlyRequest.Year,
            monthlyRequest.Month,
            request.Amount);

        await mediator.Send(command, cancellationToken);
    }
}