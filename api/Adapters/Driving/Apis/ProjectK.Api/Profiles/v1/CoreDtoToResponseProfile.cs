using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using ProjectK.Api.Dtos.v1;
using ProjectK.Api.Dtos.v1.Monthly.Responses;
using ProjectK.Core.Dtos.v1;
using ProjectK.Core.Dtos.v1.Monthly;
using ProjectK.Core.Dtos.v1.PlannedTransactions;
using MonthlyPlannedTransactionDto = ProjectK.Core.Dtos.v1.PlannedTransactions.MonthlyPlannedTransactionDto;
using SummaryDto = ProjectK.Core.Dtos.v1.Monthly.SummaryDto;

namespace ProjectK.Api.Profiles.v1;

[ExcludeFromCodeCoverage]
public class CoreDtoToResponseProfile : Profile
{
    public CoreDtoToResponseProfile()
    {
        CreateMap(typeof(PaginatedData<>), typeof(PaginatedResponse<>)).ReverseMap();
        CreateMap<PlannedTransactionDto, Dtos.v1.PlannedTransactions.Responses.PlannedTransactionDto>().ReverseMap();
        CreateMap<MonthlyPlannedTransactionDto, Dtos.v1.Monthly.Responses.MonthlyPlannedTransactionDto>()
            .ReverseMap();
        CreateMap<SummaryDto, Dtos.v1.Monthly.Responses.SummaryDto>()
            .ReverseMap();
        CreateMap<MonthlyExpensesOverviewData, MonthlyExpensesOverviewResponse>().ReverseMap();
    }
}