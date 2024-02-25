using Mediator;
using ProjectK.Core.Entities;

namespace ProjectK.Core.Queries.v1.Categories;

public sealed record GetCategoryByIdQuery(Ulid CategoryId) : IQuery<Category?>;