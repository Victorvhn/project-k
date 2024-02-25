using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ProjectK.Api.Attributes;
using ProjectK.Api.Dtos.v1;
using ProjectK.Api.Dtos.v1.Categories.Requests;
using ProjectK.Api.UseCases.v1.Categories.Interfaces;
using CategoryDto = ProjectK.Api.Dtos.v1.Categories.Responses.CategoryDto;

namespace ProjectK.Api.Controllers.v1;

[ApiVersion("1")]
public class CategoriesController(
    ICreateCategoryUseCase createCategoryUseCase,
    IUpdateCategoryUseCase updateCategoryUseCase,
    IGetPaginatedCategoriesUseCase getPaginatedCategoriesUseCase,
    IDeleteCategoryUseCase deleteCategoryUseCase,
    IGetCategoryUseCase getCategoryUseCase) : ApiControllerBase
{
    /// <summary>
    ///     Retrieves a paginated list of categories.
    /// </summary>
    /// <param name="request">Pagination parameters.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (if required).</param>
    /// <returns>The paginated list of categories.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResponse<CategoryDto>>> Get([FromQuery] PaginatedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await getPaginatedCategoriesUseCase.ExecuteAsync(request, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    ///     Creates a category.
    /// </summary>
    /// <param name="request">The category creation request.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (if required).</param>
    /// <returns>Returns the created category.</returns>
    [HttpPost]
    [Transaction]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CategoryDto>> Create([FromBody] SaveCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var response = await createCategoryUseCase.ExecuteAsync(request, cancellationToken);

        return Created("/", response);
    }

    /// <summary>
    ///     Updates a category by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the category to update.</param>
    /// <param name="request">The request containing updated category information.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (if required).</param>
    /// <returns>An <see cref="ActionResult" /> indicating the result of the update operation.</returns>
    [HttpPut("{id}")]
    [Transaction]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status204NoContent)]
    public async Task<ActionResult<CategoryDto>> Update(
        [FromRoute] Ulid id,
        [FromBody] SaveCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var category = await updateCategoryUseCase.ExecuteAsync(id, request, cancellationToken);

        return Ok(category);
    }

    /// <summary>
    ///     Deletes a category.
    /// </summary>
    /// <param name="categoryId">The ID of the category to be deleted.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (if required).</param>
    /// <returns>Returns an <see cref="ActionResult" /> indicating the result of the update operation.</returns>
    [HttpDelete("{categoryId}")]
    [Transaction]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(
        [FromRoute] Ulid categoryId,
        CancellationToken cancellationToken)
    {
        await deleteCategoryUseCase.ExecuteAsync(categoryId, cancellationToken);

        return NoContent();
    }

    /// <summary>
    ///     Retrieves a category by ID.
    /// </summary>
    /// <param name="categoryId">The ID of the category to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns the category.</returns>
    [HttpGet("{categoryId}")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CategoryDto>> GetById([FromRoute] Ulid categoryId,
        CancellationToken cancellationToken)
    {
        var result =
            await getCategoryUseCase.ExecuteAsync(categoryId, cancellationToken);

        return Ok(result);
    }
}