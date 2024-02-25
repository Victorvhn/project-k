using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using ProjectK.Api.Dtos.v1.Monthly.Requests;

namespace ProjectK.Api.ModelBinders;

public class MonthlyRequestBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        var (yearSuccessfullyParsed, monthSuccessfullyParsed) = GetValues(bindingContext, out var year, out var month);

        // Considering that the model is nullable, we don't want to bind it if the year and month are not provided.
        if (bindingContext.ModelMetadata.IsReferenceOrNullableType && !yearSuccessfullyParsed &&
            !monthSuccessfullyParsed) return Task.CompletedTask;

        // If the model is not nullable, we want to add a model error if the year and month are not provided.
        if (!yearSuccessfullyParsed) bindingContext.ModelState.TryAddModelError("year", "Invalid year format.");
        if (!monthSuccessfullyParsed) bindingContext.ModelState.TryAddModelError("month", "Invalid month format.");

        // If there are any model errors, we don't want to bind the model.
        if (!yearSuccessfullyParsed && !monthSuccessfullyParsed) return Task.CompletedTask;

        var monthlyRequest = new MonthlyRequest(year, month);

        bindingContext.Result = ModelBindingResult.Success(monthlyRequest);

        return Task.CompletedTask;
    }

    private static (bool, bool) GetValues(ModelBindingContext bindingContext, out int year, out int month)
    {
        bool yearSuccessfullyParsed;
        bool monthSuccessfullyParsed;

        var isFromQuery = bindingContext.BindingSource?.CanAcceptDataFrom(BindingSource.Query) ?? false;
        if (isFromQuery)
        {
            var queryData = bindingContext.ActionContext.HttpContext.Request.Query;
            yearSuccessfullyParsed = int.TryParse(queryData["year"], out year);
            monthSuccessfullyParsed = int.TryParse(queryData["month"], out month);

            return (yearSuccessfullyParsed, monthSuccessfullyParsed);
        }

        // Assuming that the data is coming from the path.
        var routeData = bindingContext.ActionContext.RouteData.Values;
        yearSuccessfullyParsed = int.TryParse(routeData["year"]?.ToString(), out year);
        monthSuccessfullyParsed = int.TryParse(routeData["month"]?.ToString(), out month);

        return (yearSuccessfullyParsed, monthSuccessfullyParsed);
    }
}

public class MonthlyRequestBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        return context.Metadata.ModelType == typeof(MonthlyRequest)
            ? new BinderTypeModelBinder(typeof(MonthlyRequestBinder))
            : null;
    }
}