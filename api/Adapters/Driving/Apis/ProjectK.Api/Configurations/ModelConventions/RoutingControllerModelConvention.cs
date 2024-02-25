using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using ProjectK.Api.Configurations.Extensions;

namespace ProjectK.Api.Configurations.ModelConventions;

[ExcludeFromCodeCoverage]
public class RoutingControllerModelConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        controller.ControllerName = controller.ControllerName.ToKebabCase();
    }
}