using Mediator;
using ProjectK.Core.Entities;

namespace ProjectK.Core.Commands.v1.Categories;

public sealed record UpdateCategoryCommand(
    Ulid CategoryId,
    string Name,
    string? HexColor = null
) : ICommand<Category?>;