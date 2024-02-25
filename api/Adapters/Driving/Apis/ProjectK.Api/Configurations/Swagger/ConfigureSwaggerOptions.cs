using System.Diagnostics.CodeAnalysis;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProjectK.Api.Configurations.Swagger;

[ExcludeFromCodeCoverage]
public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    : IConfigureNamedOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
            options.SwaggerDoc(
                description.GroupName,
                CreateVersionInfo(description));
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    private static OpenApiInfo CreateVersionInfo(
        ApiVersionDescription apiVersionDescription)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "";

        var info = new OpenApiInfo
        {
            Title = $"ProjectK API - {environment}",
            Version = apiVersionDescription.ApiVersion.ToString()
        };

        if (apiVersionDescription.IsDeprecated)
            info.Description +=
                " This API version has been deprecated. Please use one of the new APIs available from the explorer.";

        return info;
    }
}