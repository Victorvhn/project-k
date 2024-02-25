using System.Globalization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProjectK.Api.Filters;

public class LanguageFilter : IAsyncActionFilter
{
    private const string AcceptLanguageHeader = "Accept-Language";

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        context.HttpContext.Request.Headers.TryGetValue(AcceptLanguageHeader, out var acceptLanguage);

        var cultureInfo = IsLanguageAvailable(acceptLanguage)
            ? new CultureInfo(acceptLanguage!)
            : new CultureInfo("en-US");

        Thread.CurrentThread.CurrentCulture = cultureInfo;
        Thread.CurrentThread.CurrentUICulture = cultureInfo;

        await next();
    }

    private static bool IsLanguageAvailable(string? acceptLanguage)
    {
        if (string.IsNullOrWhiteSpace(acceptLanguage)) return false;

        var availableLanguages = new List<CultureInfo>
        {
            new("en-US"),
            new("pt-BR")
        };

        return availableLanguages.Exists(culture =>
            string.Equals(culture.Name, acceptLanguage, StringComparison.InvariantCultureIgnoreCase));
    }
}