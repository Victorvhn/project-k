using System.Diagnostics.CodeAnalysis;
using ProjectK.Api.UseCases.v1.Categories;
using ProjectK.Api.UseCases.v1.Categories.Interfaces;
using ProjectK.Api.UseCases.v1.Monthly;
using ProjectK.Api.UseCases.v1.Monthly.Interfaces;
using ProjectK.Api.UseCases.v1.PlannedTransactions;
using ProjectK.Api.UseCases.v1.PlannedTransactions.Interfaces;
using ProjectK.Api.UseCases.v1.Transactions;
using ProjectK.Api.UseCases.v1.Transactions.Interfaces;
using ProjectK.Api.UseCases.v1.Users;
using ProjectK.Api.UseCases.v1.Users.Interfaces;

namespace ProjectK.Api;

[ExcludeFromCodeCoverage]
public static class ProjectDependencies
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services)
    {
        services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
        services.AddScoped<IGetUserByEmailUseCase, GetUserByEmailUseCase>();
        services.AddScoped<ICreateCategoryUseCase, CreateCategoryUseCase>();
        services.AddScoped<IUpdateCategoryUseCase, UpdateCategoryUseCase>();
        services.AddScoped<IGetPaginatedCategoriesUseCase, GetPaginatedCategoriesUseCase>();
        services.AddScoped<ICreatePlannedTransactionUseCase, CreatePlannedTransactionUseCase>();
        services.AddScoped<IGetMonthlyPlannedTransactionsUseCase, GetMonthlyPlannedTransactionsUseCase>();
        services.AddScoped<IUpdatePlannedTransactionUseCase, UpdatePlannedTransactionUseCase>();
        services.AddScoped<IGetPlannedTransactionForDateUseCase, GetPlannedTransactionForDateUseCase>();
        services.AddScoped<IDeletePlannedTransactionUseCase, DeletePlannedTransactionUseCase>();
        services.AddScoped<IDeleteCategoryUseCase, DeleteCategoryUseCase>();
        services.AddScoped<IGetCategoryUseCase, GetCategoryUseCase>();
        services.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();
        services.AddScoped<ICreateTransactionUseCase, CreateTransactionUseCase>();
        services.AddScoped<IUpdateTransactionUseCase, UpdateTransactionUseCase>();
        services.AddScoped<IDeleteTransactionUseCase, DeleteTransactionUseCase>();
        services.AddScoped<IGetPlannedTransactionPaginatedUseCase, GetPlannedTransactionPaginatedUseCase>();
        services.AddScoped<IGetMonthlySummaryUseCase, GetMonthlySummaryUseCase>();
        services.AddScoped<IGetTransactionUseCase, GetTransactionUseCase>();
        services.AddScoped<IGetMonthlyTransactionsUseCase, GetMonthlyTransactionsUseCase>();
        services.AddScoped<IPayPlannedTransactionUseCase, PayPlannedTransactionUseCase>();
        services.AddScoped<IGetTransactionsOverviewUseCase, GetTransactionsOverviewUseCase>();
        services.AddScoped<IUpdateUserCurrencyUseCase, UpdateUserCurrencyUseCase>();

        return services;
    }
}