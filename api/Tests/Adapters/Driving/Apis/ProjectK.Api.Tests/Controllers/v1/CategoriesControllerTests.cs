using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ProjectK.Api.Controllers.v1;
using ProjectK.Api.Dtos.v1;
using ProjectK.Api.Dtos.v1.Categories.Requests;
using ProjectK.Api.UseCases.v1.Categories.Interfaces;
using ProjectK.Tests.Shared;
using CategoryDto = ProjectK.Api.Dtos.v1.Categories.Responses.CategoryDto;

namespace ProjectK.Api.Tests.Controllers.v1;

public class CategoriesControllerTests
{
    private readonly CategoriesController _categoriesController;

    private readonly ICreateCategoryUseCase _createCategoryUseCase;
    private readonly IDeleteCategoryUseCase _deleteCategoryUseCase;
    private readonly Fixture _fixture = CustomAutoFixture.Create();
    private readonly IGetCategoryUseCase _getCategoryUseCase;
    private readonly IGetPaginatedCategoriesUseCase _getPaginatedCategoriesUseCase;
    private readonly IUpdateCategoryUseCase _updateCategoryUseCase;

    public CategoriesControllerTests()
    {
        _createCategoryUseCase = Substitute.For<ICreateCategoryUseCase>();
        _updateCategoryUseCase = Substitute.For<IUpdateCategoryUseCase>();
        _getPaginatedCategoriesUseCase = Substitute.For<IGetPaginatedCategoriesUseCase>();
        _deleteCategoryUseCase = Substitute.For<IDeleteCategoryUseCase>();
        _getCategoryUseCase = Substitute.For<IGetCategoryUseCase>();

        _categoriesController = new CategoriesController(_createCategoryUseCase, _updateCategoryUseCase,
            _getPaginatedCategoriesUseCase, _deleteCategoryUseCase, _getCategoryUseCase);
    }

    [Fact]
    public async Task Create_Should_Return_Created()
    {
        var request = _fixture.Create<SaveCategoryRequest>();
        var cancellationToken = new CancellationToken();
        var useCaseResult = _fixture.Create<CategoryDto>();

        _createCategoryUseCase
            .ExecuteAsync(request, cancellationToken)
            .Returns(useCaseResult);

        var actionResult = await _categoriesController.Create(request, cancellationToken);

        var result = actionResult.Result as CreatedResult;
        result
            .Should()
            .NotBeNull();
        result!
            .Value
            .Should()
            .BeEquivalentTo(useCaseResult);
    }

    [Fact]
    public async Task Update_Should_Return_Ok()
    {
        var categoryId = Ulid.NewUlid();
        var request = _fixture.Create<SaveCategoryRequest>();
        var cancellationToken = new CancellationToken();
        var useCaseResult = _fixture.Create<CategoryDto>();

        _updateCategoryUseCase
            .ExecuteAsync(categoryId, request, cancellationToken)
            .Returns(useCaseResult);

        var actionResult = await _categoriesController.Update(categoryId, request, cancellationToken);

        var result = actionResult.Result as OkObjectResult;
        result
            .Should()
            .NotBeNull();
        result!
            .Value
            .Should()
            .BeEquivalentTo(useCaseResult);
    }

    [Fact]
    public async Task Get_Should_Return_Ok()
    {
        var request = _fixture.Create<PaginatedRequest>();
        var cancellationToken = new CancellationToken();
        var useCaseResult = _fixture.Create<PaginatedResponse<CategoryDto>>();

        _getPaginatedCategoriesUseCase
            .ExecuteAsync(request, cancellationToken)
            .Returns(useCaseResult);

        var actionResult = await _categoriesController.Get(request, cancellationToken);

        var result = actionResult.Result as OkObjectResult;
        result
            .Should()
            .NotBeNull();
        result!
            .Value
            .Should()
            .BeEquivalentTo(useCaseResult);
    }

    [Fact]
    public async Task Delete_Should_Return_NoContent()
    {
        var categoryId = _fixture.Create<Ulid>();
        var cancellationToken = new CancellationToken();

        var actionResult = await _categoriesController.Delete(categoryId, cancellationToken);

        var result = actionResult as NoContentResult;
        result
            .Should()
            .NotBeNull();
        await _deleteCategoryUseCase
            .Received(1)
            .ExecuteAsync(categoryId, cancellationToken);
    }

    [Fact]
    public async Task GetById_Should_Return_Ok()
    {
        var categoryId = _fixture.Create<Ulid>();
        var cancellationToken = new CancellationToken();
        var useCaseResult = _fixture.Create<CategoryDto>();
        _getCategoryUseCase
            .ExecuteAsync(categoryId, cancellationToken)
            .Returns(useCaseResult);

        var actionResult = await _categoriesController.GetById(categoryId, cancellationToken);

        var result = actionResult.Result as OkObjectResult;
        result
            .Should()
            .NotBeNull();
        result!
            .Value
            .Should()
            .BeEquivalentTo(useCaseResult);
    }
}