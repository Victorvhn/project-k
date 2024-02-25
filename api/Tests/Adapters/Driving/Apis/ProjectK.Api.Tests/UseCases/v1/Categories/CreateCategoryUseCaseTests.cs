using AutoFixture;
using FluentAssertions;
using Mediator;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using ProjectK.Api.Dtos.v1.Categories.Requests;
using ProjectK.Api.UseCases.v1.Categories;
using ProjectK.Api.UseCases.v1.Categories.Interfaces;
using ProjectK.Core.Commands.v1.Categories;
using ProjectK.Core.Entities;
using ProjectK.Tests.Shared;

namespace ProjectK.Api.Tests.UseCases.v1.Categories;

public class CreateCategoryUseCaseTests
{
    private static readonly Fixture Fixture = CustomAutoFixture.Create();

    private readonly ICreateCategoryUseCase _createCategoryUseCase;
    private readonly ISender _sender;

    public CreateCategoryUseCaseTests()
    {
        var mapper = MapperFixture.GetMapper();
        _sender = Substitute.For<ISender>();

        _createCategoryUseCase = new CreateCategoryUseCase(mapper, _sender);
    }

    [Fact]
    public async Task It_should_create_a_category()
    {
        // Arrange
        var request = Fixture.Create<SaveCategoryRequest>();
        var category = Fixture.Create<Category>();

        _sender
            .Send(Arg.Is<CreateCategoryCommand>(a =>
                a.Name == request.Name &&
                a.HexColor == request.HexColor
            ))
            .Returns(category);

        // Act
        var result = await _createCategoryUseCase.ExecuteAsync(request);

        // Assert
        result
            .Should()
            .BeEquivalentTo(category, opt => opt.ExcludingMissingMembers());
    }

    [Fact]
    public async Task It_should_return_null_when_service_returns_null()
    {
        // Arrange
        var request = Fixture.Create<SaveCategoryRequest>();

        _sender
            .Send(Arg.Is<CreateCategoryCommand>(a =>
                a.Name == request.Name &&
                a.HexColor == request.HexColor
            ))
            .ReturnsNull();

        // Act
        var result = await _createCategoryUseCase.ExecuteAsync(request);

        // Assert
        result
            .Should()
            .BeNull();
    }
}