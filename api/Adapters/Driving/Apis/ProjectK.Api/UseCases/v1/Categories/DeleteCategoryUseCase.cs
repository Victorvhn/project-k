using Mediator;
using ProjectK.Api.UseCases.v1.Categories.Interfaces;
using ProjectK.Core.Commands.v1.Categories;

namespace ProjectK.Api.UseCases.v1.Categories;

internal class DeleteCategoryUseCase(ISender mediator) : IDeleteCategoryUseCase
{
    public async Task ExecuteAsync(Ulid categoryId, CancellationToken cancellationToken = default)
    {
        var command = new DeleteCategoryCommand(categoryId);

        await mediator.Send(command, cancellationToken);
    }
}