using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using ProjectK.Api.Dtos.v1.Categories.Responses;
using ProjectK.Api.Dtos.v1.Monthly.Responses;
using ProjectK.Api.Dtos.v1.PlannedTransactions.Responses;
using ProjectK.Api.Dtos.v1.Transactions.Responses;
using ProjectK.Api.Dtos.v1.Users.Responses;
using ProjectK.Core.Entities;

namespace ProjectK.Api.Profiles.v1;

[ExcludeFromCodeCoverage]
public class EntityToResponseProfile : Profile
{
    public EntityToResponseProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<PlannedTransaction, PlannedTransactionDto>().ReverseMap();
        CreateMap<Transaction, TransactionDto>().ReverseMap();
        CreateMap<Transaction, MonthlyTransactionDto>().ReverseMap();
    }
}