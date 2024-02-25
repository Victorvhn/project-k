using FluentAssertions;
using ProjectK.Core.Dtos.v1.Monthly;

namespace ProjectK.Core.Tests.Dtos.v1.Monthly;

public class SummaryDtoTests
{
    [Theory]
    [InlineData(100, 100, 0, 100)]
    [InlineData(0, 0, 0, 100)]
    [InlineData(0, 1000, 1000, 1000)]
    [InlineData(0, 10000, 10000, 10000)]
    [InlineData(500, 250, -250, 50)]
    [InlineData(1000, 250, -750, 25)]
    [InlineData(1000, 0, -1000, 0)]
    [InlineData(800, 400, -400, 50)]
    [InlineData(100, 0, -100, 0)]
    [InlineData(100, 50, -50, 50)]
    [InlineData(100, 150, 50, 150)]
    public void It_should_build_correctly(decimal expected, decimal current, decimal difference, decimal percentage)
    {
        // Act
        var summaryDto = new SummaryDto(expected, current);

        // Assert
        summaryDto
            .Expected
            .Should()
            .Be(expected);
        summaryDto
            .Current
            .Should()
            .Be(current);
        summaryDto
            .Difference
            .Should()
            .Be(difference);
        summaryDto
            .Percentage
            .Should()
            .Be(percentage);
    }
}