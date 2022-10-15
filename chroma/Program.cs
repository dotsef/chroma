using System.Diagnostics;
using System.Reflection;

using Chroma;

if (args is ["--version"])
{
    var version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
    Debug.Assert(version.ProductVersion is not null, "The version should be known at runtime");
    Write(version.ProductVersion, Color.Random);
}

if (args.Length is not 1)
{
    Write("Please provide a single color in hex format.", Color.Red);
    return -1;
}

var hexcode = args[0];

if (!Color.TryParse(hexcode, null, out var color))
{
    Write($"'{hexcode}' is not a parsable hex-formatted color", Color.Red);
    return -1;
}

Write($"rgb({color.R}, {color.G}, {color.B})", color);
return 0;

static void Write(string text, in Color color)
{
    var length = text.Length + 6 + 10 + 3 + color.R.DigitCount() + color.G.DigitCount() + color.B.DigitCount();
    var buffer = string.Create(length, (color, text: text.AsMemory()), (span, data) =>
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
    Console.WriteLine(buffer);
}
