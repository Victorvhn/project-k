using AutoFixture;
using FluentAssertions;
using NSubstitute;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Dtos.v1.PlannedTransactions;
using ProjectK.Core.Entities;
using ProjectK.Core.Handlers.v1.Summary;
using ProjectK.Core.Queries.v1.Summary;
using ProjectK.Core.Resource;
using ProjectK.Tests.Shared;
using ProjectK.Tests.Shared.Builders.Entities;

namespace ProjectK.Core.Tests.Handlers.v1.Summary;

public class GetMonthlyOverviewByCategoryQueryHandlerTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();
    private readonly ICategoryRepository _categoryRepository;

    private readonly GetMonthlyOverviewByCategoryQueryHandler _commandHandler;
    private readonly IPlannedTransactionRepository _plannedTransactionRepository;

    private readonly ITransactionRepository _transactionRepository;

    public GetMonthlyOverviewByCategoryQueryHandlerTests()
    {
        _transactionRepository = Substitute.For<ITransactionRepository>();
        _plannedTransactionRepository = Substitute.For<IPlannedTransactionRepository>();
        _categoryRepository = Substitute.For<ICategoryRepository>();

        _commandHandler = new GetMonthlyOverviewByCategoryQueryHandler(_transactionRepository,
            _plannedTransactionRepository, _categoryRepository);
    }

    [Fact]
    public async Task Handle_should_return_monthly_expenses_overview()
    {
        // Arrange
        var query = Fixture.Create<GetMonthlyOverviewByCategoryQuery>();

        var category1 = new CategoryBuilder()
            .WithId(Ulid.NewUlid())
            .WithName("Category 1")
            .Build();

        var category2 = new CategoryBuilder()
            .WithId(Ulid.NewUlid())
            .WithName("Category 2")
            .Build();

        var category3 = new CategoryBuilder()
            .WithId(Ulid.NewUlid())
            .WithName("Category 3")
            .Build();

        var plannedTransaction1 = Fixture.Build<MonthlyPlannedTransactionDto>()
            .With(w => w.Amount, 200)
            .With(w => w.CategoryId, category1.Id)
            .Create();

        var plannedTransaction2 = Fixture.Build<MonthlyPlannedTransactionDto>()
            .With(w => w.Amount, 400)
            .With(w => w.CategoryId, category1.Id)
            .Create();

        var plannedTransaction3 = Fixture.Build<MonthlyPlannedTransactionDto>()
            .With(w => w.Amount, 1000)
            .With(w => w.CategoryId, (Ulid?)null)
            .Create();

        var plannedTransaction4 = Fixture.Build<MonthlyPlannedTransactionDto>()
            .With(w => w.Amount, 1000)
            .With(w => w.CategoryId, category2.Id)
            .Create();

        var transaction1 = new TransactionBuilder()
            .WithCategory(category1)
            .WithAmount(100)
            .Build();
        var transaction2 = new TransactionBuilder()
            .WithCategory(category1)
            .WithAmount(900)
            .Build();
        var transaction3 = new TransactionBuilder()
            .WithoutCategory()
            .WithAmount(500)
            .Build();
        var transaction4 = new TransactionBuilder()
            .WithCategory(category3)
            .WithAmount(400)
            .Build();

        _plannedTransactionRepository
            .GetMonthlyAsync(Arg.Is<MonthlyFilter>(a => a.Year == query.Year && a.Month == query.Month))
            .Returns(new List<MonthlyPlannedTransactionDto>
                { plannedTransaction1, plannedTransaction2, plannedTransaction3, plannedTransaction4 });

        _transactionRepository
            .GetMonthlyAsync(Arg.Is<MonthlyFilter>(a => a.Year == query.Year && a.Month == query.Month))
            .Returns(new List<Transaction> { transaction1, transaction2, transaction3, transaction4 });

        _categoryRepository
            .GetByIdsAsync(
                Arg.Is<Ulid[]>(a => a.Contains(category1.Id) && a.Contains(category2.Id) && a.Contains(category3.Id)))
            .Returns(new[] { category1, category2, category3 });

        // Act
        var result = await _commandHandler.Handle(query, CancellationToken.None);

        // Assert
        result
            .Should()
            .SatisfyRespectively(
                item1 =>
                {
                    item1
                        .CategoryName
                        .Should()
                        .Be(category1.Name);
                    item1
                        .CategoryHexColor
                        .Should()
                        .Be(category1.HexColor);
                    item1
                        .PlannedAmount
                        .Should()
                        .Be(plannedTransaction1.Amount + plannedTransaction2.Amount);
                    item1
                        .CurrentAmount
                        .Should()
                        .Be(transaction1.Amount + transaction2.Amount);
                },
                item2 =>
                {
                    item2
                        .CategoryName
                        .Should()
                        .Be(category2.Name);
                    item2
                        .CategoryHexColor
                        .Should()
                        .Be(category2.HexColor);
                    item2
                        .PlannedAmount
                        .Should()
                        .Be(plannedTransaction4.Amount);
                    item2
                        .CurrentAmount
                        .Should()
                        .Be(0);
                },
                item3 =>
                {
                    item3
                        .CategoryName
                        .Should()
                        .Be(category3.Name);
                    item3
                        .CategoryHexColor
                        .Should()
                        .Be(category3.HexColor);
                    item3
                        .PlannedAmount
                        .Should()
                        .Be(0);
                    item3
                        .CurrentAmount
                        .Should()
                        .Be(transaction4.Amount);
                },
                item4 =>
                {
                    item4
                        .CategoryName
                        .Should()
                        .Be(Resources.DefaultCategory);
                    item4
                        .CategoryHexColor
                        .Should()
                        .Be(Category.DefaultColor);
                    item4
                        .PlannedAmount
                        .Should()
                        .Be(plannedTransaction3.Amount);
                    item4
                        .CurrentAmount
                        .Should()
                        .Be(transaction3.Amount);
                });
    }
}