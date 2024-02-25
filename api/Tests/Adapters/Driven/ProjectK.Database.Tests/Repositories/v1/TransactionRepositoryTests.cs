using AutoFixture;
using FluentAssertions;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Entities;
using ProjectK.Core.Enums;
using ProjectK.Database.Repositories.v1;
using ProjectK.Tests.Shared;
using ProjectK.Tests.Shared.Builders.Entities;

namespace ProjectK.Database.Tests.Repositories.v1;

public class TransactionRepositoryTests : SqLiteIntegrationTest
{
    private readonly Fixture _fixture = CustomAutoFixture.Create();

    [Fact]
    public async Task GetCurrentAmountSummaryByDateAsync_should_return_current_amount_summary()
    {
        var transaction1 = new TransactionBuilder()
            .WithPaidAt(new DateOnly(2022, 12, 02))
            .WithType(TransactionType.Expense)
            .WithAmount(100)
            .Build();

        var transaction2 = new TransactionBuilder()
            .WithPaidAt(new DateOnly(2023, 12, 02))
            .WithType(TransactionType.Expense)
            .WithAmount(200)
            .Build();

        var transaction3 = new TransactionBuilder()
            .WithPaidAt(new DateOnly(2024, 01, 02))
            .WithType(TransactionType.Expense)
            .WithAmount(1000)
            .Build();

        var transaction4 = new TransactionBuilder()
            .WithPaidAt(new DateOnly(2024, 01, 20))
            .WithType(TransactionType.Expense)
            .WithAmount(500)
            .Build();

        var transaction5 = new TransactionBuilder()
            .WithPaidAt(new DateOnly(2024, 02, 15))
            .WithType(TransactionType.Expense)
            .WithAmount(954)
            .Build();

        var transaction6 = new TransactionBuilder()
            .WithPaidAt(new DateOnly(2024, 01, 15))
            .WithType(TransactionType.Income)
            .WithAmount(954)
            .Build();

        var monthlyFilter = _fixture.Build<MonthlyFilter>()
            .With(w => w.Month, 01)
            .With(w => w.Year, 2024)
            .Create();

        await AddEntities(transaction1, transaction2, transaction3, transaction4, transaction5, transaction6);

        var result = await ExecuteCommand(async context =>
        {
            var repository = new TransactionRepository(context);

            return await repository.GetCurrentAmountSummaryByDateAsync(monthlyFilter, TransactionType.Expense);
        });

        result
            .Should()
            .Be(1500);
    }

    [Fact]
    public async Task GetMonthlyAsync_should_return_transactions()
    {
        var transaction1 = new TransactionBuilder()
            .WithPaidAt(new DateOnly(2022, 12, 02))
            .WithType(TransactionType.Expense)
            .WithAmount(100)
            .Build();

        var transaction2 = new TransactionBuilder()
            .WithPaidAt(new DateOnly(2023, 12, 02))
            .WithType(TransactionType.Expense)
            .WithAmount(200)
            .Build();

        var transaction3 = new TransactionBuilder()
            .WithPaidAt(new DateOnly(2024, 01, 02))
            .WithType(TransactionType.Expense)
            .WithAmount(1000)
            .Build();

        var transaction4 = new TransactionBuilder()
            .WithPaidAt(new DateOnly(2024, 01, 20))
            .WithType(TransactionType.Expense)
            .WithAmount(500)
            .Build();

        var transaction5 = new TransactionBuilder()
            .WithPaidAt(new DateOnly(2024, 02, 15))
            .WithType(TransactionType.Expense)
            .WithAmount(954)
            .Build();

        var transaction6 = new TransactionBuilder()
            .WithPaidAt(new DateOnly(2024, 01, 15))
            .WithType(TransactionType.Income)
            .WithAmount(954)
            .Build();

        var monthlyFilter = _fixture.Build<MonthlyFilter>()
            .With(w => w.Month, 01)
            .With(w => w.Year, 2024)
            .Create();

        await AddEntities(transaction1, transaction2, transaction3, transaction4, transaction5, transaction6);

        var result = await ExecuteCommand(async context =>
        {
            var repository = new TransactionRepository(context);

            return await repository.GetMonthlyAsync(monthlyFilter);
        });

        result
            .Should()
            .BeEquivalentTo(new List<Transaction>
            {
                transaction3,
                transaction4,
                transaction6
            }, opt => opt.ExcludingMissingMembers().Excluding(e => e.Category).Excluding(e => e.PlannedTransaction));
    }
}