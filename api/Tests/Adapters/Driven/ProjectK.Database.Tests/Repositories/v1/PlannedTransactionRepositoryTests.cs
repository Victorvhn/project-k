using AutoFixture;
using FluentAssertions;
using ProjectK.Core.Dtos.v1;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Entities;
using ProjectK.Core.Enums;
using ProjectK.Core.Resource;
using ProjectK.Database.Repositories.v1;
using ProjectK.Tests.Shared;
using ProjectK.Tests.Shared.Builders.Entities;

namespace ProjectK.Database.Tests.Repositories.v1;

public class PlannedTransactionRepositoryTests : SqLiteIntegrationTest
{
    private readonly Fixture _fixture = CustomAutoFixture.Create();

    [Fact]
    public async Task GetMonthlyAsync_should_return_monthly_data()
    {
        var category = CategoryBuilder.Create();

        var planned1 = new PlannedTransactionBuilder()
            .WithStartsAt(new DateOnly(2023, 12, 01))
            .WithEndsAt(new DateOnly(2023, 12, 01))
            .WithCategory(category)
            .WithRecurrence(Recurrence.Monthly)
            .WithType(TransactionType.Income)
            .WithAmountType(AmountType.Variable)
            .Build();

        var transaction1 = new TransactionBuilder()
            .WithPlannedTransaction(planned1)
            .WithPaidAt(new DateOnly(2023, 12, 16))
            .WithPaidAt(new DateOnly(2023, 12, 15))
            .WithAmount(planned1.Amount + 1)
            .Build();

        var customPlanned1 = new CustomPlannedTransactionBuilder()
            .WithRefersTo(new DateOnly(2023, 12, 01))
            .WithBasePlannedTransaction(planned1)
            .Build();

        var planned2 = new PlannedTransactionBuilder()
            .WithStartsAt(new DateOnly(2023, 10, 01))
            .WithEndsAt(new DateOnly(2023, 11, 01))
            .WithCategory(category)
            .WithRecurrence(Recurrence.Monthly)
            .Build();

        var planned3 = new PlannedTransactionBuilder()
            .WithStartsAt(new DateOnly(2024, 01, 01))
            .WithEndsAt(new DateOnly(2024, 05, 01))
            .WithCategory(category)
            .WithRecurrence(Recurrence.Monthly)
            .Build();

        var planned4 = new PlannedTransactionBuilder()
            .WithStartsAt(new DateOnly(2023, 01, 02))
            .WithoutEndsAt()
            .WithCategory(category)
            .WithRecurrence(Recurrence.Monthly)
            .WithType(TransactionType.Expense)
            .WithAmountType(AmountType.Fixed)
            .Build();

        var planned5 = new PlannedTransactionBuilder()
            .WithStartsAt(new DateOnly(2023, 12, 03))
            .WithoutEndsAt()
            .WithCategory(category)
            .WithRecurrence(Recurrence.Annual)
            .WithType(TransactionType.Expense)
            .WithAmountType(AmountType.Variable)
            .Build();

        var planned6 = new PlannedTransactionBuilder()
            .WithStartsAt(new DateOnly(2023, 11, 03))
            .WithoutEndsAt()
            .WithCategory(category)
            .WithRecurrence(Recurrence.Annual)
            .Build();

        await AddEntities(category, planned1, customPlanned1, transaction1, planned2, planned3, planned4, planned5,
            planned6);

        var result = (await ExecuteCommand(async context =>
        {
            var repository = new PlannedTransactionRepository(context);

            return await repository.GetMonthlyAsync(new MonthlyFilter(2023, 12));
        })).ToList();

        result
            .Should()
            .SatisfyRespectively(item1 =>
                {
                    item1
                        .Id
                        .Should()
                        .Be(planned1.Id);
                    item1
                        .Description
                        .Should()
                        .Be(customPlanned1.Description);
                    item1
                        .Amount
                        .Should()
                        .BeApproximately(customPlanned1.Amount, 0.01M);
                    item1
                        .AmountType
                        .Should()
                        .Be(planned1.AmountType);
                    item1
                        .Type
                        .Should()
                        .Be(planned1.Type);
                    item1
                        .Recurrence
                        .Should()
                        .Be(planned1.Recurrence);
                    item1
                        .StartsAt
                        .Should()
                        .Be(customPlanned1.RefersTo);
                    item1
                        .EndsAt
                        .Should()
                        .Be(customPlanned1.RefersTo);
                    item1
                        .CategoryId
                        .Should()
                        .Be(planned1.CategoryId);
                    item1
                        .Paid
                        .Should()
                        .BeTrue();
                    item1
                        .PaidAt
                        .Should()
                        .Be(transaction1.PaidAt);
                    item1
                        .PaidAmount
                        .Should()
                        .BeApproximately(transaction1.Amount, 0.01M);
                    item1.Tags
                        .Should()
                        .BeEquivalentTo(new List<string>
                        {
                            category.Name,
                            Resources.TransactionType_Income,
                            Resources.Recurrence_Monthly,
                            Resources.AmountType_Variable,
                            Resources.Overpaid,
                            Resources.CustomPlannedTransaction
                        });
                },
                item2 =>
                {
                    item2
                        .Id
                        .Should()
                        .Be(planned4.Id);
                    item2
                        .Description
                        .Should()
                        .Be(planned4.Description);
                    item2
                        .Amount
                        .Should()
                        .BeApproximately(planned4.Amount, 0.01M);
                    item2
                        .AmountType
                        .Should()
                        .Be(planned4.AmountType);
                    item2
                        .Type
                        .Should()
                        .Be(planned4.Type);
                    item2
                        .Recurrence
                        .Should()
                        .Be(planned4.Recurrence);
                    item2
                        .StartsAt
                        .Should()
                        .Be(planned4.StartsAt);
                    item2
                        .EndsAt
                        .Should()
                        .Be(planned4.EndsAt);
                    item2
                        .CategoryId
                        .Should()
                        .Be(planned4.CategoryId);
                    item2
                        .Paid
                        .Should()
                        .BeFalse();
                    item2
                        .PaidAt
                        .Should()
                        .BeNull();
                    item2
                        .PaidAmount
                        .Should()
                        .BeNull();
                    item2.Tags
                        .Should()
                        .BeEquivalentTo(new List<string>
                        {
                            category.Name,
                            Resources.TransactionType_Expense,
                            Resources.Recurrence_Monthly,
                            Resources.AmountType_Fixed
                        });
                },
                item3 =>
                {
                    item3
                        .Id
                        .Should()
                        .Be(planned5.Id);
                    item3
                        .Description
                        .Should()
                        .Be(planned5.Description);
                    item3
                        .Amount
                        .Should()
                        .BeApproximately(planned5.Amount, 0.01M);
                    item3
                        .AmountType
                        .Should()
                        .Be(planned5.AmountType);
                    item3
                        .Type
                        .Should()
                        .Be(planned5.Type);
                    item3
                        .Recurrence
                        .Should()
                        .Be(planned5.Recurrence);
                    item3
                        .StartsAt
                        .Should()
                        .Be(planned5.StartsAt);
                    item3
                        .EndsAt
                        .Should()
                        .Be(planned5.EndsAt);
                    item3
                        .CategoryId
                        .Should()
                        .Be(planned5.CategoryId);
                    item3
                        .Paid
                        .Should()
                        .BeFalse();
                    item3
                        .PaidAt
                        .Should()
                        .BeNull();
                    item3
                        .PaidAmount
                        .Should()
                        .BeNull();
                    item3.Tags
                        .Should()
                        .BeEquivalentTo(new List<string>
                        {
                            category.Name,
                            Resources.TransactionType_Expense,
                            Resources.Recurrence_Annual,
                            Resources.AmountType_Variable
                        });
                });
    }

    [Fact]
    public async Task GetByIdWithCustomPlannedAsync_should_return_planned_transaction_with_custom_planned_transactions()
    {
        var category = CategoryBuilder.Create();

        var planned1 = new PlannedTransactionBuilder()
            .WithStartsAt(new DateOnly(2023, 12, 02))
            .WithEndsAt(new DateOnly(2024, 12, 02))
            .WithCategory(category)
            .WithRecurrence(Recurrence.Monthly)
            .Build();

        var customPlanned1 = new CustomPlannedTransactionBuilder()
            .WithRefersTo(new DateOnly(2024, 01, 02))
            .WithBasePlannedTransaction(planned1)
            .Build();

        var planned2 = new PlannedTransactionBuilder()
            .WithStartsAt(new DateOnly(2023, 10, 05))
            .WithEndsAt(new DateOnly(2025, 10, 05))
            .WithCategory(category)
            .WithRecurrence(Recurrence.Monthly)
            .Build();

        await AddEntities(category, planned1, customPlanned1, planned2);

        var result = await ExecuteCommand(async context =>
        {
            var repository = new PlannedTransactionRepository(context);

            return await repository.GetByIdWithCustomPlannedAsync(planned1.Id);
        });

        result
            .Should()
            .NotBeNull();
        result
            .Should()
            .BeEquivalentTo(planned1,
                opt => opt.Using<decimal>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.01M))
                    .WhenTypeIs<decimal>().Excluding(e => e.Category).Excluding(e => e.CustomPlannedTransactions));
        result!
            .CustomPlannedTransactions
            .Should()
            .AllBeEquivalentTo(customPlanned1,
                opt => opt.Using<decimal>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.01M))
                    .WhenTypeIs<decimal>().Excluding(e => e.BasePlannedTransaction));
    }

    [Fact]
    public async Task
        GetByIdWithAllCustomPlannedBeforeDateAsync_should_return_planned_transaction_with_all_custom_planned_transactions_before_date()
    {
        var category = CategoryBuilder.Create();

        var planned1 = new PlannedTransactionBuilder()
            .WithStartsAt(new DateOnly(2023, 12, 02))
            .WithEndsAt(new DateOnly(2024, 12, 02))
            .WithCategory(category)
            .WithRecurrence(Recurrence.Monthly)
            .Build();

        var customPlanned1 = new CustomPlannedTransactionBuilder()
            .WithRefersTo(new DateOnly(2024, 01, 02))
            .WithBasePlannedTransaction(planned1)
            .Build();

        var planned2 = new PlannedTransactionBuilder()
            .WithStartsAt(new DateOnly(2023, 10, 05))
            .WithEndsAt(new DateOnly(2025, 10, 05))
            .WithCategory(category)
            .WithRecurrence(Recurrence.Monthly)
            .Build();

        var monthlyFilter = _fixture.Build<MonthlyFilter>()
            .With(w => w.Month, 01)
            .With(w => w.Year, 2024)
            .Create();

        await AddEntities(category, planned1, customPlanned1, planned2);

        var result = await ExecuteCommand(async context =>
        {
            var repository = new PlannedTransactionRepository(context);

            return await repository.GetByIdWithAllCustomPlannedBeforeDateAsync(planned1.Id, monthlyFilter);
        });

        result
            .Should()
            .NotBeNull();
        result
            .Should()
            .BeEquivalentTo(planned1,
                opt => opt.Using<decimal>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.01M))
                    .WhenTypeIs<decimal>().Excluding(e => e.Category).Excluding(e => e.CustomPlannedTransactions));
        result!
            .CustomPlannedTransactions
            .Should()
            .AllBeEquivalentTo(customPlanned1,
                opt => opt.Using<decimal>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.01M))
                    .WhenTypeIs<decimal>().Excluding(e => e.BasePlannedTransaction));
    }

    [Fact]
    public async Task
        GetByIdAndDateWithCustomPlannedAsync_should_return_planned_transaction_with_custom_planned_transactions()
    {
        var category = CategoryBuilder.Create();

        var planned1 = new PlannedTransactionBuilder()
            .WithStartsAt(new DateOnly(2023, 12, 02))
            .WithEndsAt(new DateOnly(2024, 12, 02))
            .WithCategory(category)
            .WithRecurrence(Recurrence.Monthly)
            .Build();

        var customPlanned1 = new CustomPlannedTransactionBuilder()
            .WithRefersTo(new DateOnly(2024, 01, 02))
            .WithBasePlannedTransaction(planned1)
            .Build();

        var planned2 = new PlannedTransactionBuilder()
            .WithStartsAt(new DateOnly(2023, 10, 05))
            .WithEndsAt(new DateOnly(2025, 10, 05))
            .WithCategory(category)
            .WithRecurrence(Recurrence.Monthly)
            .Build();

        var monthlyFilter = _fixture.Build<MonthlyFilter>()
            .With(w => w.Month, 01)
            .With(w => w.Year, 2024)
            .Create();

        await AddEntities(category, planned1, customPlanned1, planned2);

        var result = await ExecuteCommand(async context =>
        {
            var repository = new PlannedTransactionRepository(context);

            return await repository.GetByIdAndDateWithCustomPlannedAsync(planned1.Id, monthlyFilter);
        });

        result
            .Should()
            .NotBeNull();
        result
            .Should()
            .BeEquivalentTo(planned1,
                opt => opt.Using<decimal>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.01M))
                    .WhenTypeIs<decimal>().Excluding(e => e.Category).Excluding(e => e.CustomPlannedTransactions));
        result!
            .CustomPlannedTransactions
            .Should()
            .AllBeEquivalentTo(customPlanned1,
                opt => opt.Using<decimal>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.01M))
                    .WhenTypeIs<decimal>().Excluding(e => e.BasePlannedTransaction));
    }

    [Fact]
    public async Task GetPaginatedAsync_should_return_paginated_data()
    {
        var planned1 = new PlannedTransactionBuilder()
            .WithDescription("1")
            .WithStartsAt(new DateOnly(2023, 12, 01))
            .WithEndsAt(new DateOnly(2023, 12, 01))
            .WithRecurrence(Recurrence.Monthly)
            .Build();

        var planned2 = new PlannedTransactionBuilder()
            .WithDescription("2")
            .WithStartsAt(new DateOnly(2023, 10, 01))
            .WithEndsAt(new DateOnly(2023, 11, 01))
            .WithRecurrence(Recurrence.Monthly)
            .Build();

        var planned3 = new PlannedTransactionBuilder()
            .WithDescription("3")
            .WithStartsAt(new DateOnly(2024, 01, 01))
            .WithEndsAt(new DateOnly(2024, 05, 01))
            .WithRecurrence(Recurrence.Monthly)
            .Build();

        var planned4 = new PlannedTransactionBuilder()
            .WithDescription("4")
            .WithStartsAt(new DateOnly(2023, 01, 02))
            .WithoutEndsAt()
            .WithRecurrence(Recurrence.Monthly)
            .Build();

        var planned5 = new PlannedTransactionBuilder()
            .WithDescription("5")
            .WithStartsAt(new DateOnly(2023, 12, 03))
            .WithoutEndsAt()
            .WithRecurrence(Recurrence.Annual)
            .Build();

        var planned6 = new PlannedTransactionBuilder()
            .WithDescription("6")
            .WithStartsAt(new DateOnly(2023, 11, 03))
            .WithoutEndsAt()
            .WithRecurrence(Recurrence.Annual)
            .Build();

        await AddEntities(planned1, planned2, planned3, planned4, planned5, planned6);

        var result = await ExecuteCommand(async context =>
        {
            var repository = new PlannedTransactionRepository(context);

            return await repository.GetPaginatedAsync(new PaginationFilter(5, 1));
        });

        result
            .totalCount
            .Should()
            .Be(6);
        result
            .plannedTransactions
            .Should()
            .BeEquivalentTo(new List<PlannedTransaction>
                    { planned1, planned2, planned3, planned4, planned5 },
                opt => opt
                    .Using<decimal>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.01M))
                    .WhenTypeIs<decimal>()
                    .Excluding(e => e.Category)
                    .Excluding(e => e.CustomPlannedTransactions)
                    .Excluding(e => e.Transactions));
    }

    [Fact]
    public async Task GetExpectedAmountSummaryByDateAsync_should_return_expected_amount_summary()
    {
        var planned1 = new PlannedTransactionBuilder()
            .WithStartsAt(new DateOnly(2022, 12, 02))
            .WithEndsAt(new DateOnly(2024, 12, 02))
            .WithoutCategory()
            .WithRecurrence(Recurrence.Monthly)
            .WithType(TransactionType.Expense)
            .WithAmount(100)
            .Build();

        var customPlanned1 = new CustomPlannedTransactionBuilder()
            .WithRefersTo(new DateOnly(2023, 12, 02))
            .WithBasePlannedTransaction(planned1)
            .WithAmount(150)
            .Build();
        var customPlanned2 = new CustomPlannedTransactionBuilder()
            .WithRefersTo(new DateOnly(2024, 01, 02))
            .WithBasePlannedTransaction(planned1)
            .WithAmount(200)
            .Build();

        var planned2 = new PlannedTransactionBuilder()
            .WithStartsAt(new DateOnly(2023, 10, 05))
            .WithEndsAt(new DateOnly(2025, 10, 05))
            .WithoutCategory()
            .WithRecurrence(Recurrence.Monthly)
            .WithType(TransactionType.Expense)
            .WithAmount(200)
            .Build();

        var monthlyFilter = _fixture.Build<MonthlyFilter>()
            .With(w => w.Month, 01)
            .With(w => w.Year, 2024)
            .Create();

        await AddEntities(planned1, customPlanned1, customPlanned2, planned2);

        var result = await ExecuteCommand(async context =>
        {
            var repository = new PlannedTransactionRepository(context);

            return await repository.GetExpectedAmountSummaryByDateAsync(monthlyFilter, TransactionType.Expense);
        });

        result
            .Should()
            .Be(400);
    }
}