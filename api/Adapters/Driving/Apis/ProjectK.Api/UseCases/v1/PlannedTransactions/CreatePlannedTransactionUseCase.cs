using AutoMapper;
using Mediator;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Requests;
using ProjectK.Api.UseCases.v1.PlannedTransactions.Interfaces;
using ProjectK.Core.Commands.v1.PlannedTransactions;
using PlannedTransactionDto = ProjectK.Api.Dtos.v1.PlannedTransactions.Responses.PlannedTransactionDto;

namespace ProjectK.Api.UseCases.v1.PlannedTransactions;

internal class CreatePlannedTransactionUseCase(
    IMapper mapper,
    ISender mediator) : ICreatePlannedTransactionUseCase
{
    public async Task<PlannedTransactionDto?> ExecuteAsync(CreatePlannedTransactionRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreatePlannedTransactionCommand(
            request.Description,
            request.Amount,
            request.AmountType,
            request.Type,
            request.Recurrence,
            request.StartsAt,
            request.EndsAt,
            request.CategoryId);

        var result = await mediator.Send(command, cancellationToken);

        return mapper.Map<PlannedTransactionDto?>(result);
    }
}