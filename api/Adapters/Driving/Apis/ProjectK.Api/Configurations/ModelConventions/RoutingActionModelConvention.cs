using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using ProjectK.Api.Configurations.Extensions;

namespace ProjectK.Api.Configurations.ModelConventions;

[ExcludeFromCodeCoverage]
public class RoutingActionModelConvention : IActionModelConvention
{
    public void Apply(ActionModel action)
    {
        action.ActionName = action.ActionName.ToKebabCase();
    }
}