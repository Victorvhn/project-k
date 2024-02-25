using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using ProjectK.Api.Dtos.v1;
using ProjectK.Api.Dtos.v1.Monthly.Requests;
using ProjectK.Core.Dtos.v1;
using ProjectK.Core.Dtos.v1.Monthly;

namespace ProjectK.Api.Profiles.v1;

[ExcludeFromCodeCoverage]
public class RequestToDomainDtoProfile : Profile
{
    public RequestToDomainDtoProfile()
    {
        CreateMap<PaginatedRequest, PaginationFilter>().ReverseMap();
        CreateMap<MonthlyRequest, MonthlyFilter>().ReverseMap();
    }
}