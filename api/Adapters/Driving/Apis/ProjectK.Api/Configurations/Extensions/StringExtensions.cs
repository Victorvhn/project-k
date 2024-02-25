using System.Text;

namespace ProjectK.Api.Configurations.Extensions;

public static class StringExtensions
{
    public static string ToKebabCase(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        var stringBuilder = new StringBuilder();
        for (var i = 0; i < text.Length; i++)
            if (char.IsUpper(text[i]))
            {
                if (i > 0 && text[i - 1] != '-')
                    stringBuilder.Append('-');

                stringBuilder.Append(char.ToLower(text[i]));
            }
            else if (char.IsLetterOrDigit(text[i]))
            {
                stringBuilder.Append(text[i]);
            }

        return stringBuilder.ToString();
    }
}