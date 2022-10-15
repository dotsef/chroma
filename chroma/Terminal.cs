namespace Chroma;

public static class Terminal
{
    public static void WriteLine(string text, in Color color)
    {
        var colorizedText = ANSIColorize(text, color);
        Console.WriteLine(colorizedText);
    }

    public static string ANSIColorize(string text, in Color color)
    {
        var length = text.Length + 6 + 10 + 3 + color.R.DigitCount() + color.G.DigitCount() + color.B.DigitCount();
        return string.Create(length, (color, text: text.AsMemory()), (span, data) =>
        {
            span[0] = '\u001b';
            span[1] = '[';
            span[2] = '3';
            span[3] = '8';
            span[4] = ';';
            span[5] = '2';
            span[6] = ';';

            var rgbSpan = new RGBSpan(data.color);
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

            var text = data.text;

            text.Span.CopyTo(span[i..(i + text.Length)]);
            i += text.Length;

            span[i++] = '\u001b';
            span[i++] = '[';
            span[i++] = '0';
            span[i++] = 'm';
        });
    }
}
