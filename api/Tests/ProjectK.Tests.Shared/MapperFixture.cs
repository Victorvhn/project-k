using AutoMapper;
using ProjectK.Api.Profiles.v1;

namespace ProjectK.Tests.Shared;

public static class MapperFixture
{
    public static Mapper GetMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RequestToDomainDtoProfile>();
            cfg.AddProfile<EntityToResponseProfile>();
            cfg.AddProfile<CoreDtoToResponseProfile>();
        });
        var mapper = new Mapper(config);

        return mapper;
    }
}