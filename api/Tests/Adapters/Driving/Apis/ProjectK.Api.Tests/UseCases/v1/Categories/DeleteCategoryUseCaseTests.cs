using AutoFixture;
using Mediator;
using NSubstitute;
using ProjectK.Api.UseCases.v1.Categories;
using ProjectK.Core.Commands.v1.Categories;
using ProjectK.Tests.Shared;

namespace ProjectK.Api.Tests.UseCases.v1.Categories;

public class DeleteCategoryUseCaseTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly DeleteCategoryUseCase _deleteCategoryUseCase;
    private readonly ISender _sender;

    public DeleteCategoryUseCaseTests()
    {
        _sender = Substitute.For<ISender>();

        _deleteCategoryUseCase = new DeleteCategoryUseCase(_sender);
    }

    [Fact]
    public async Task It_should_delete_a_category()
    {
        // Arrange
        var categoryId = Fixture.Create<Ulid>();

        // Act
        await _deleteCategoryUseCase.ExecuteAsync(categoryId);

        // Assert
        await _sender
            .Received(1)
            .Send(Arg.Is<DeleteCategoryCommand>(a => a.CategoryId == categoryId),
                Arg.Any<CancellationToken>());
    }
}