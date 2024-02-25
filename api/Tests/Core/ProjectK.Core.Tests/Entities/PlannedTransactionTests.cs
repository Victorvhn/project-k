using FluentAssertions;
using ProjectK.Core.Entities;
using ProjectK.Core.Enums;
using ProjectK.Core.Resource;
using ProjectK.Tests.Shared.Builders.Entities;

namespace ProjectK.Core.Tests.Entities;

public class PlannedTransactionTests
{
    [Theory]
    [MemberData(nameof(GetTags_Data))]
    public void GetTags_ShouldReturnTags(PlannedTransaction plannedTransactionTests, string[] expectedTags)
    {
        // Act
        var tags = plannedTransactionTests.GetTags();

        // Assert
        tags
            .Should()
            .BeEquivalentTo(expectedTags);
    }

    public static TheoryData<PlannedTransaction, string[]> GetTags_Data()
    {
        var theoryData = new TheoryData<PlannedTransaction, string[]>
        {
            {
                new PlannedTransactionBuilder()
                    .WithCategory(new CategoryBuilder()
                        .WithName("CategoryName")
                        .Build())
                    .WithType(TransactionType.Expense)
                    .WithRecurrence(Recurrence.Monthly)
                    .WithAmountType(AmountType.Variable)
                    .WithoutCustomPlannedTransactions()
                    .WithoutTransactions()
                    .Build(),
                [
                    "CategoryName", Resources.TransactionType_Expense, Resources.Recurrence_Monthly,
                    Resources.AmountType_Variable
                ]
            },
            {
                new PlannedTransactionBuilder()
                    .WithoutCategory()
                    .WithType(TransactionType.Income)
                    .WithRecurrence(Recurrence.Annual)
                    .WithAmountType(AmountType.Fixed)
                    .WithAmount(150)
                    .WithoutCustomPlannedTransactions()
                    .WithTransaction(
                        new TransactionBuilder()
                            .WithAmount(100)
                            .Build()
                    )
                    .Build(),
                [
                    Resources.TransactionType_Income, Resources.Recurrence_Annual,
                    Resources.AmountType_Fixed, Resources.Underpaid
                ]
            },
            {
                new PlannedTransactionBuilder()
                    .WithoutCategory()
                    .WithType(TransactionType.Income)
                    .WithRecurrence(Recurrence.Annual)
                    .WithAmountType(AmountType.Fixed)
                    .WithAmount(100)
                    .WithoutCustomPlannedTransactions()
                    .WithTransaction(
                        new TransactionBuilder()
                            .WithAmount(150)
                            .Build()
                    )
                    .Build(),
                [
                    Resources.TransactionType_Income, Resources.Recurrence_Annual,
                    Resources.AmountType_Fixed, Resources.Overpaid
                ]
            },
            {
                new PlannedTransactionBuilder()
                    .WithoutCategory()
                    .WithType(TransactionType.Income)
                    .WithRecurrence(Recurrence.Annual)
                    .WithAmountType(AmountType.Fixed)
                    .WithAmount(100)
                    .WithTransaction(
                        new TransactionBuilder()
                            .WithAmount(150)
                            .Build()
                    )
                    .WithCustomPlannedTransactions(
                        new CustomPlannedTransactionBuilder()
                            .WithAmount(150)
                            .Build()
                    )
                    .Build(),
                [
                    Resources.TransactionType_Income, Resources.Recurrence_Annual,
                    Resources.AmountType_Fixed, Resources.CustomPlannedTransaction
                ]
            },
            {
                new PlannedTransactionBuilder()
                    .WithoutCategory()
                    .WithType(TransactionType.Income)
                    .WithRecurrence(Recurrence.Annual)
                    .WithAmountType(AmountType.Fixed)
                    .WithAmount(200)
                    .WithTransaction(
                        new TransactionBuilder()
                            .WithAmount(100)
                            .Build()
                    )
                    .WithCustomPlannedTransactions(
                        new CustomPlannedTransactionBuilder()
                            .WithAmount(150)
                            .Build()
                    )
                    .Build(),
                [
                    Resources.TransactionType_Income, Resources.Recurrence_Annual,
                    Resources.AmountType_Fixed, Resources.Underpaid, Resources.CustomPlannedTransaction
                ]
            }
        };

        return theoryData;
    }
}