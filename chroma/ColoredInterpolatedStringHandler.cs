using System.Globalization;
using System.Runtime.CompilerServices;

namespace Chroma;

[InterpolatedStringHandler]
public ref struct ColoredInterpolatedStringHandler
{
    private DefaultInterpolatedStringHandler _defaultHandler;

    public ColoredInterpolatedStringHandler(int literalLength, int formattedCount, out bool shouldAppend)
    {
        _defaultHandler = new(literalLength, formattedCount);
        shouldAppend = true;
    }

    public void AppendLiteral(string literal)
        => _defaultHandler.AppendLiteral(literal);

    public void AppendFormatted<T>(T value)
        => _defaultHandler.AppendFormatted(value);

    public void AppendFormatted<T>(T value, string format)
    {
        if (Color.TryParse(format, CultureInfo.InvariantCulture, out var color))
            _defaultHandler.AppendFormatted(Terminal.Colorize(value?.ToString() ?? "null", color));
        else if (value is IFormattable formattable)
            _defaultHandler.AppendFormatted(formattable, format);
        else
            _defaultHandler.AppendFormatted(value);
    }

    public void AppendFormatted<T>(T value, int alignment) where T : IFormattable
        => _defaultHandler.AppendFormatted(value, alignment);

    public void AppendFormatted<T>(T value, int alignment, string? format) where T : IFormattable
        => _defaultHandler.AppendFormatted(value, alignment, format);

    public void AppendFormatted(ReadOnlySpan<char> value)
        => _defaultHandler.AppendFormatted(value);

    public void AppendFormatted(ReadOnlySpan<char> value, int alignment = 0, string? format = null)
        => _defaultHandler.AppendFormatted(value, alignment, format);

    public string ToStringAndClear()
        => _defaultHandler.ToStringAndClear();
}
