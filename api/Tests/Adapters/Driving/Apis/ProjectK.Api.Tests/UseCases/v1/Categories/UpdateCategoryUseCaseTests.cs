using AutoFixture;
using FluentAssertions;
using Mediator;
using NSubstitute;
using ProjectK.Api.Dtos.v1.Categories.Requests;
using ProjectK.Api.UseCases.v1.Categories;
using ProjectK.Core.Commands.v1.Categories;
using ProjectK.Tests.Shared;
using ProjectK.Tests.Shared.Builders.Entities;

namespace ProjectK.Api.Tests.UseCases.v1.Categories;

public class UpdateCategoryUseCaseTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();
    private readonly ISender _sender;

    private readonly UpdateCategoryUseCase _updateCategoryUseCase;

    public UpdateCategoryUseCaseTests()
    {
        var mapper = MapperFixture.GetMapper();
        _sender = Substitute.For<ISender>();

        _updateCategoryUseCase = new UpdateCategoryUseCase(mapper, _sender);
    }

    [Fact]
    public async Task It_should_update_a_category()
    {
        // Arrange
        var id = Fixture.Create<Ulid>();
        var request = Fixture.Create<SaveCategoryRequest>();
        var category = new CategoryBuilder()
            .Build();

        _sender
            .Send(Arg.Is<UpdateCategoryCommand>(a =>
                a.CategoryId == id &&
                a.Name == request.Name &&
                a.HexColor == request.HexColor
            ))
            .Returns(category);

        // Act
        var result = await _updateCategoryUseCase.ExecuteAsync(id, request);

        // Assert
        result
            .Should()
            .BeEquivalentTo(category, options => options.ExcludingMissingMembers());
    }
}