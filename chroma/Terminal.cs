using System.Globalization;
using System.Runtime.CompilerServices;

namespace Chroma;

[InterpolatedStringHandler]
public ref struct ColoredInterpolatedStringHandler
{
#pragma warning disable IDE0044 // This MUST be mutable, not sure why.
    private DefaultInterpolatedStringHandler _defaultHandler;
#pragma warning restore IDE0044

    public ColoredInterpolatedStringHandler(int literalLength, int formattedCount, out bool shouldAppend)
    {
        _defaultHandler = new(literalLength, formattedCount);
        shouldAppend = true;
    }

    // MANDATORY METHOD: The compiler assumes this method exists to construct the string.
    public void AppendLiteral(string literal)
    {
        _defaultHandler.AppendLiteral(literal);
    }

    public void AppendFormatted<T>(T value, string format)
    {
        if (Color.TryParse(format, CultureInfo.InvariantCulture, out var color))
        {
            var text = value?.ToString() ?? "null";
            var length = text.Length + 6 + 10 + 3 + color.R.DigitCount() + color.G.DigitCount() + color.B.DigitCount();
            Span<char> span = stackalloc char[length];
            Terminal.ANSIColorize(text, color, span);
            _defaultHandler.AppendFormatted(span);
        }
        else if (value is IFormattable formattable)
        {
            _defaultHandler.AppendFormatted(formattable, format);
        }
        else
        {
            _defaultHandler.AppendFormatted(value);
        }
    }

    // MANDATORY METHOD: The compiler assumes this method exists to construct the string.
    public void AppendFormatted<T>(T value)
    {
        _defaultHandler.AppendFormatted(value);
    }

    // MANDATORY: The compiler eventually calls ToString() on the interpolated string handler, so this is where the final construction is done.
    public override string ToString()
    {
        return _defaultHandler.ToString();
    }

    // MANDATORY: The compiler eventually calls ToString() on the interpolated string handler, so this is where the final construction is done. 
    public string ToStringAndClear()
    {
        return _defaultHandler.ToStringAndClear();
    }
}

public static class Terminal
{
    public static void WriteLine(in ReadOnlySpan<char> text, in Color color, int indent = 0)
    {
        var length = indent + text.Length + 6 + 10 + 3 + color.R.DigitCount() + color.G.DigitCount() + color.B.DigitCount();
        Span<char> span = stackalloc char[length];
        
        if (indent > 0)
            span[..indent].Fill(' ');

        ANSIColorize(text, color, span[indent..]);

        Console.Out.WriteLine(span);
    }

    public static void Write(ref ColoredInterpolatedStringHandler handler)
    {
        Console.Out.WriteLine(handler.ToString());
    }

    public static void WriteLine(in ReadOnlySpan<char> text)
        => Console.Out.WriteLine(text);

    public static void ANSIColorize(in ReadOnlySpan<char> text, in Color color, in Span<char> span)
    {
        span[0] = '\u001b';
        span[1] = '[';
        span[2] = '3';
        span[3] = '8';
        span[4] = ';';
        span[5] = '2';
        span[6] = ';';

        var rgbSpan = new RGBSpan(color);
        var i = 7;
        rgbSpan.R.CopyTo(span[i..(i + rgbSpan.R.Length)]);
        i += rgbSpan.R.Length;
        span[i++] = ';';
        rgbSpan.G.CopyTo(span[i..(i + rgbSpan.G.Length)]);
        i += rgbSpan.G.Length;
        span[i++] = ';';
        rgbSpan.B.CopyTo(span[i..(i + rgbSpan.B.Length)]);
        i += rgbSpan.B.Length;
        span[i++] = 'm';

        text.CopyTo(span[i..(i + text.Length)]);
        i += text.Length;

        span[i++] = '\u001b';
        span[i++] = '[';
        span[i++] = '0';
        span[i++] = 'm';
    }
}
