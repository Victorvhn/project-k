using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectK.Api.Attributes;
using ProjectK.Api.Dtos.v1.Users.Requests;
using ProjectK.Api.Dtos.v1.Users.Responses;
using ProjectK.Api.UseCases.v1.Users.Interfaces;

namespace ProjectK.Api.Controllers.v1;

[ApiVersion("1")]
public class UsersController(
    ICreateUserUseCase createUserUseCase,
    IGetUserByEmailUseCase getUserByEmailUseCase,
    IDeleteUserUseCase deleteUserUseCase,
    IUpdateUserCurrencyUseCase updateUserCurrencyUseCase) : ApiControllerBase
{
    /// <summary>
    ///     Creates a user.
    /// </summary>
    /// <param name="request">The user creation request.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (if required).</param>
    /// <returns>Returns the created user.</returns>
    [AllowAnonymous]
    [HttpPost]
    [Transaction]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var response = await createUserUseCase.ExecuteAsync(request, cancellationToken);

        return Created("/", response);
    }

    /// <summary>
    ///     Verifies the existence of a user based on their email.
    /// </summary>
    /// <param name="email">The email of the user to be checked.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (if required).</param>
    /// <returns>Ok if the user exists; otherwise, NotFound.</returns>
    [AllowAnonymous]
    [HttpGet("{email}/[action]")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDto>> Exists([FromRoute] string email,
        CancellationToken cancellationToken)
    {
        var result = await getUserByEmailUseCase.ExecuteAsync(email, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    ///     Deletes a user.
    /// </summary>
    /// <param name="userId">The ID of the user to be deleted.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (if required).</param>
    /// <returns>Returns an <see cref="ActionResult" /> indicating the result of the update operation.</returns>
    [HttpDelete("{userId}")]
    [Transaction]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(
        [FromRoute] Ulid userId,
        CancellationToken cancellationToken)
    {
        await deleteUserUseCase.ExecuteAsync(userId, cancellationToken);

        return NoContent();
    }
    
    /// <summary>
    ///     Updates user's currency.
    /// </summary>
    /// <param name="userId">The ID of the user to be patched.</param>
    /// <param name="request">The data to update.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation (if required).</param>
    /// <returns>Returns an <see cref="ActionResult" /> indicating the result of the update operation.</returns>
    [HttpPatch("{userId}")]
    [Transaction]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDto>> UpdateCurrency(
        [FromRoute] Ulid userId,
        [FromBody] UpdateCurrencyRequest request,
        CancellationToken cancellationToken)
    {
        var userDto = await updateUserCurrencyUseCase.ExecuteAsync(userId, request, cancellationToken);

        return Ok(userDto);
    }
}