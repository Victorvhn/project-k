using Mediator;

namespace ProjectK.Core.Commands.v1.Categories;

public sealed record DeleteCategoryCommand(
    Ulid CategoryId
) : ICommand;