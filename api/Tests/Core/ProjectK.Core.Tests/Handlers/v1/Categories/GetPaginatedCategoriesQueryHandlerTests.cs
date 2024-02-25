using AutoFixture;
using FluentAssertions;
using NSubstitute;
using ProjectK.Core.Adapters.Driven.Database.Repositories.v1;
using ProjectK.Core.Dtos.v1;
using ProjectK.Core.Entities;
using ProjectK.Core.Handlers.v1.Categories;
using ProjectK.Core.Queries.v1.Categories;
using ProjectK.Tests.Shared;

namespace ProjectK.Core.Tests.Handlers.v1.Categories;

public class GetPaginatedCategoriesQueryHandlerTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly ICategoryRepository _categoryRepository;

    private readonly GetPaginatedCategoriesQueryHandler _commandHandler;

    public GetPaginatedCategoriesQueryHandlerTests()
    {
        _categoryRepository = Substitute.For<ICategoryRepository>();

        _commandHandler = new GetPaginatedCategoriesQueryHandler(_categoryRepository);
    }

    [Fact]
    public async Task Handle_should_return_a_paginated_list_of_categories()
    {
        // Arrange
        var query = Fixture.Create<GetPaginatedCategoriesQuery>();
        var categories = Fixture.CreateMany<Category>().ToList();
        var totalCount = Fixture.Create<int>();

        _categoryRepository
            .GetPaginatedAsync(query)
            .Returns((categories, totalCount));

        // Act
        var result = await _commandHandler.Handle(query, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeEquivalentTo(new PaginatedData<Category>(categories, totalCount, query.PageSize, query.CurrentPage));
    }
}