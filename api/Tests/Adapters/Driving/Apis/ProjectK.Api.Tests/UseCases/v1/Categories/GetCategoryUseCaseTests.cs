using AutoFixture;
using FluentAssertions;
using Mediator;
using NSubstitute;
using ProjectK.Api.UseCases.v1.Categories;
using ProjectK.Api.UseCases.v1.Categories.Interfaces;
using ProjectK.Core.Queries.v1.Categories;
using ProjectK.Tests.Shared;
using ProjectK.Tests.Shared.Builders.Entities;

namespace ProjectK.Api.Tests.UseCases.v1.Categories;

public class GetCategoryUseCaseTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly IGetCategoryUseCase _getCategoryUseCase;
    private readonly ISender _sender;

    public GetCategoryUseCaseTests()
    {
        var mapper = MapperFixture.GetMapper();
        _sender = Substitute.For<ISender>();

        _getCategoryUseCase = new GetCategoryUseCase(mapper, _sender);
    }

    [Fact]
    public async Task It_should_get_a_category_by_id()
    {
        // Arrange
        var categoryId = Fixture.Create<Ulid>();
        var category = new CategoryBuilder()
            .WithId(categoryId)
            .Build();

        _sender
            .Send(Arg.Is<GetCategoryByIdQuery>(a => a.CategoryId == categoryId))
            .Returns(category);

        // Act
        var result = await _getCategoryUseCase.ExecuteAsync(categoryId);

        // Assert
        result
            .Should()
            .BeEquivalentTo(category, opt => opt.ExcludingMissingMembers());
    }
}