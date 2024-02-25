using Mediator;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Queries.v1.Summary;

namespace ProjectK.Core.Handlers.v1.Summary;

internal sealed class GetMonthlySummaryQueryHandler(
    ITransactionRepository transactionRepository,
    IPlannedTransactionRepository plannedTransactionRepository) : IQueryHandler<GetMonthlySummaryQuery, SummaryDto>
{
    public async ValueTask<SummaryDto> Handle(GetMonthlySummaryQuery request, CancellationToken cancellationToken)
    {
        var filter = new MonthlyFilter(request.Year, request.Month);

        var expectedAmount =
            await plannedTransactionRepository
                .GetExpectedAmountSummaryByDateAsync(filter, request.TransactionType, cancellationToken);

        var actualAmount = await transactionRepository
            .GetCurrentAmountSummaryByDateAsync(filter, request.TransactionType, cancellationToken);

        return new SummaryDto(expectedAmount, actualAmount);
    }
}