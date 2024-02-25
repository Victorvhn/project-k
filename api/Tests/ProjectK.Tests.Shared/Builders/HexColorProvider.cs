namespace ProjectK.Tests.Shared.Builders;

public static class HexColorProvider
{
    public static string Random()
    {
        var random = new Random();

        var r = (byte)random.Next(256);
        var g = (byte)random.Next(256);
        var b = (byte)random.Next(256);

        return $"#{r:X2}{g:X2}{b:X2}";
    }
}