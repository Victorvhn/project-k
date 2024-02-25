using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ProjectK.Database.Converters;

public class UlidToBytesConverter : ValueConverter<Ulid, byte[]>
{
    private static readonly ConverterMappingHints DefaultHints = new(16);

    public UlidToBytesConverter() : this(null)
    {
    }

    public UlidToBytesConverter(ConverterMappingHints? mappingHints)
        : base(
            x => x.ToByteArray(),
            x => new Ulid(x),
            DefaultHints.With(mappingHints))
    {
    }
}

public class UlidToStringConverter : ValueConverter<Ulid, string>
{
    private static readonly ConverterMappingHints DefaultHints = new(26);

    public UlidToStringConverter() : this(null)
    {
    }

    public UlidToStringConverter(ConverterMappingHints? mappingHints)
        : base(
            x => x.ToString(),
            x => Ulid.Parse(x),
            DefaultHints.With(mappingHints))
    {
    }
}