using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProjectK.Api.SchemaFilters;

public class UlidSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(Ulid)) return;

        schema.Type = "string";
        schema.Format = "ulid";
        schema.Pattern = "^[0-9A-Z]{26}$";
        schema.Description = "Universally Unique Lexicographically Sortable Identifier";
    }
}