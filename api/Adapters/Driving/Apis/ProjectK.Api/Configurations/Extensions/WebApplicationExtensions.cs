using System.Diagnostics.CodeAnalysis;
using Asp.Versioning.ApiExplorer;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using ProjectK.Api.Middlewares;

namespace ProjectK.Api.Configurations.Extensions;

[ExcludeFromCodeCoverage]
public static class WebApplicationExtensions
{
    public static void UseHealthChecks(
        this WebApplication application)
    {
        application.MapHealthChecks("/healthz", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }

    public static void UseSwaggerWithVersions(
        this WebApplication application)
    {
        var apiVersionDescriptionProvider = application.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        application.UseSwagger();
        application.UseSwaggerUI(options =>
        {
            foreach (var apiVersionDescription in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
                options.SwaggerEndpoint(
                    $"/swagger/{apiVersionDescription.GroupName}/swagger.json",
                    apiVersionDescription.GroupName.ToUpperInvariant()
                );
            options.EnablePersistAuthorization();
        });
    }

    public static IApplicationBuilder UseRequestContextLogging(
        this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestContextLoggingMiddleware>();

        return app;
    }
}