using FluentAssertions;
using ProjectK.Core.Queries.v1.Categories;
using ProjectK.Database.Repositories.v1;
using ProjectK.Tests.Shared.Builders.Entities;

namespace ProjectK.Database.Tests.Repositories.v1;

public class CategoryRepositoryTests : SqLiteIntegrationTest
{
    [Fact]
    public async Task ExistsByNameAsync_should_return_true_when_name_exists()
    {
        var category = CategoryBuilder
            .Create();

        await AddEntities(category);

        var result = await ExecuteCommand(async context =>
        {
            var repository = new CategoryRepository(context);

            return await repository.ExistsByNameAsync(category.Name);
        });

        result
            .Should()
            .BeTrue();
    }

    [Fact]
    public async Task ExistsByNameAsync_should_return_false_when_name_does_not_exist()
    {
        var result = await ExecuteCommand(async context =>
        {
            var repository = new CategoryRepository(context);

            return await repository.ExistsByNameAsync("Category");
        });

        result
            .Should()
            .BeFalse();
    }

    [Fact]
    public async Task GetPaginatedAsync_should_return_paginated_data()
    {
        var categories = CategoryBuilder.CreateMany(10);

        await AddEntities(categories);

        var result = await ExecuteCommand(async context =>
        {
            var repository = new CategoryRepository(context);

            return await repository.GetPaginatedAsync(new GetPaginatedCategoriesQuery(1, 5));
        });

        result
            .Should()
            .NotBeNull();

        result
            .totalCount
            .Should()
            .Be(10);

        result
            .categories
            .Should()
            .BeEquivalentTo(categories
                .OrderBy(o => o.Name)
                .Take(5)
            );
    }
}