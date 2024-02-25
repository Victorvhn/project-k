using AutoFixture;
using AutoFixture.Kernel;
using ProjectK.Api.Dtos.v1.Monthly.Requests;
using ProjectK.Core.Dtos.v1.Monthly;

namespace ProjectK.Tests.Shared;

public static class CustomAutoFixture
{
    public static Fixture Create()
    {
        var fixture = new Fixture();

        fixture.Customizations.Add(new MonthlyFilterSpecimenBuilder());
        fixture.Customize<DateOnly>(composer => composer.FromFactory(new ValidDateOnlySpecimenBuilder()));
        fixture.Customize<Ulid>(composer => composer.FromFactory(new UlidSpecimenBuilder()));

        return fixture;
    }
}

public class UlidSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(Ulid)) return new NoSpecimen();

        return Ulid.NewUlid();
    }
}

public class ValidDateOnlySpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(DateOnly)) return new NoSpecimen();

        return DateOnly.FromDateTime(DateTime.Today);
    }
}

public class MonthlyFilterSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type && type == typeof(MonthlyFilter))
        {        
            return new MonthlyFilter(DateTime.Today.Year, DateTime.Today.Month);
        }

        return new NoSpecimen();
    }
}