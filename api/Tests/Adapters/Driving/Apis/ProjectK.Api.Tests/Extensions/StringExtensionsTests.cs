using FluentAssertions;
using ProjectK.Api.Configurations.Extensions;

namespace ProjectK.Api.Tests.Extensions;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("ToKebabCase", "to-kebab-case")]
    [InlineData("To Kebab Case", "to-kebab-case")]
    [InlineData("", "")]
    public void ToKebabCase_should_convert_string_to_KebabCase(string text, string expected)
    {
        // Act
        var result = text.ToKebabCase();

        // Assert
        result.Should().Be(expected);
    }
}