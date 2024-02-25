using AutoFixture;
using FluentAssertions;
using Mediator;
using NSubstitute;
using ProjectK.Api.Dtos.v1;
using ProjectK.Api.UseCases.v1.Categories;
using ProjectK.Api.UseCases.v1.Categories.Interfaces;
using ProjectK.Core.Dtos.v1;
using ProjectK.Core.Entities;
using ProjectK.Core.Queries.v1.Categories;
using ProjectK.Tests.Shared;

namespace ProjectK.Api.Tests.UseCases.v1.Categories;

public class GetPaginatedCategoriesUseCaseTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly IGetPaginatedCategoriesUseCase _getPaginatedCategoriesUseCase;
    private readonly ISender _sender;

    public GetPaginatedCategoriesUseCaseTests()
    {
        var mapper = MapperFixture.GetMapper();
        _sender = Substitute.For<ISender>();

        _getPaginatedCategoriesUseCase = new GetPaginatedCategoriesUseCase(mapper, _sender);
    }

    [Fact]
    public async Task It_should_return_paginated_categories()
    {
        // Arrange
        var request = Fixture.Create<PaginatedRequest>();
        var paginatedResult = Fixture.Create<PaginatedData<Category>>();

        _sender
            .Send(Arg.Is<GetPaginatedCategoriesQuery>(a =>
                a.CurrentPage == request.CurrentPage &&
                a.PageSize == request.PageSize
            ))
            .Returns(paginatedResult);

        // Act
        var result = await _getPaginatedCategoriesUseCase.ExecuteAsync(request);

        // Assert
        result
            .Should()
            .BeEquivalentTo(paginatedResult, opt => opt.ExcludingMissingMembers());
    }
}