namespace Chroma;

public static class Terminal
{
    public static void WriteLine(in ReadOnlySpan<char> text, in Color color, int indent = 0)
    {
        var length = text.Length + 6 + 10 + 3 + color.R.DigitCount() + color.G.DigitCount() + color.B.DigitCount();
        Span<char> span = stackalloc char[length];
        ANSIColorize(text, color, ref span);
        if (indent > 0)
        {
            Span<char> tabs = stackalloc char[indent];
            tabs.Fill(' ');
            Console.Out.Write(tabs);
        }
            
        Console.Out.WriteLine(span);
    }

    public static void WriteLine(in ReadOnlySpan<char> text)
        => Console.Out.WriteLine(text);

    public static void ANSIColorize(in ReadOnlySpan<char> text, in Color color, ref Span<char> span)
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
