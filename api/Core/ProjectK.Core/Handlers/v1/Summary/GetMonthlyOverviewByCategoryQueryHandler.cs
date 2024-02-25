using Mediator;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Dtos.v1.PlannedTransactions;
using ProjectK.Core.Entities;
using ProjectK.Core.Queries.v1.Summary;
using ProjectK.Core.Resource;

namespace ProjectK.Core.Handlers.v1.Summary;

internal sealed class GetMonthlyOverviewByCategoryQueryHandler(
    ITransactionRepository transactionRepository,
    IPlannedTransactionRepository plannedTransactionRepository,
    ICategoryRepository categoryRepository)
    : IQueryHandler<GetMonthlyOverviewByCategoryQuery, IEnumerable<MonthlyExpensesOverviewData>>
{
    public async ValueTask<IEnumerable<MonthlyExpensesOverviewData>> Handle(GetMonthlyOverviewByCategoryQuery request,
        CancellationToken cancellationToken)
    {
        var filter = new MonthlyFilter(request.Year, request.Month);

        var plannedTransactionsByCategory = await GetPlannedTransactionsByCategory(filter, cancellationToken);
        var transactionsByCategory = await GetTransactionsByCategory(filter, cancellationToken);

        var categories = await GetCategories(plannedTransactionsByCategory, transactionsByCategory, cancellationToken);

        var result = GetTransactionOverview(categories, plannedTransactionsByCategory, transactionsByCategory);

        return result;
    }

    private static IEnumerable<MonthlyExpensesOverviewData> GetTransactionOverview(List<Category?> categories,
        IReadOnlyCollection<IGrouping<Ulid?, MonthlyPlannedTransactionDto>> plannedTransactionsByCategory,
        IReadOnlyCollection<IGrouping<Ulid?, Transaction>> transactionsByCategory)
    {
        foreach (var category in categories)
        {
            var plannedAmount = plannedTransactionsByCategory
                .FirstOrDefault(f => f.Key == category?.Id)?
                .Sum(s => s.Amount) ?? 0;
            var currentAmount = transactionsByCategory
                .FirstOrDefault(f => f.Key == category?.Id)?
                .Sum(s => s.Amount) ?? 0;

            var categoryName = category?.Name ?? Resources.DefaultCategory;
            var categoryColor = category?.HexColor ?? Category.DefaultColor;
            
            yield return new MonthlyExpensesOverviewData(categoryName, categoryColor, plannedAmount,
                currentAmount);
        }
    }

    private async Task<List<Category?>> GetCategories(
        IEnumerable<IGrouping<Ulid?, MonthlyPlannedTransactionDto>> plannedTransactionsByCategory,
        IEnumerable<IGrouping<Ulid?, Transaction>> transactionsByCategory, CancellationToken cancellationToken)
    {
        var availableCategories = new List<Ulid>();
        availableCategories.AddRange(plannedTransactionsByCategory
            .Where(w => w.Key != null)
            .Select(s => (Ulid)s.Key!));
        availableCategories.AddRange(transactionsByCategory
            .Where(w => w.Key != null)
            .Select(s => (Ulid)s.Key!));
        availableCategories = availableCategories.Distinct().ToList();

        List<Category?> categories =
            (await categoryRepository.GetByIdsAsync(availableCategories.ToArray(), cancellationToken))
            .OrderBy(o => o.Name)
            .ToList()!;
        categories.Add(null);

        return categories;
    }

    private async Task<List<IGrouping<Ulid?, Transaction>>> GetTransactionsByCategory(MonthlyFilter filter,
        CancellationToken cancellationToken)
    {
        var transactions = await transactionRepository.GetMonthlyAsync(filter, cancellationToken);

        var transactionsByCategory = transactions
            .GroupBy(g => g.CategoryId)
            .ToList();

        return transactionsByCategory;
    }

    private async Task<List<IGrouping<Ulid?, MonthlyPlannedTransactionDto>>> GetPlannedTransactionsByCategory(
        MonthlyFilter filter, CancellationToken cancellationToken)
    {
        var plannedTransactions = await plannedTransactionRepository.GetMonthlyAsync(filter, cancellationToken);

        var plannedTransactionsByCategory = plannedTransactions
            .GroupBy(g => g.CategoryId)
            .ToList();

        return plannedTransactionsByCategory;
    }
}