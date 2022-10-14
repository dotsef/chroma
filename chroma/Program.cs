using System.Diagnostics;
using System.Globalization;
using System.Reflection;

if (args is ["--version"])
{
    var version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
    Write($"chroma cli version {version.ProductVersion}", new Color
    {
        R = 255 - 11,
        G = 10 * 16 + 2,
        B = 6 * 16 + 1
    });
}

if (args.Length is not 1)
{
    Write("Please provide a single color in hex format.", new Color { R = 255, G = 0, B = 0 });
    return -1;
}

var hexcode = args[0].Trim().TrimStart('#');

var color = hexcode.Length switch
{
    3 => ParseFrom3(hexcode),
    4 => ParseFrom4(hexcode),
    6 => ParseFrom6(hexcode),
    8 => ParseFrom8(hexcode),
    _ => throw new InvalidOperationException($"'{hexcode}' can not be parsed to a color.")
};

Write($"rgb({color.R}, {color.G}, {color.B})", color);
return 0;
unsafe Color ParseFrom3(string hexcode)
{
    Span<char> bytes = stackalloc char[8];
    bytes[0] = bytes[1] = hexcode[0];
    bytes[2] = bytes[3] = hexcode[1];
    bytes[4] = bytes[5] = hexcode[2];
    bytes[6] = bytes[7] = 'F';

    return ParseFromSpan8(bytes);
}

Color ParseFrom4(string hexcode)
{
    Span<char> bytes = stackalloc char[8];
    bytes[0] = bytes[1] = hexcode[0];
    bytes[2] = bytes[3] = hexcode[1];
    bytes[4] = bytes[5] = hexcode[2];
    bytes[6] = bytes[7] = hexcode[3];

    return ParseFromSpan8(hexcode);
}

Color ParseFrom6(string hexcode)
{
    Span<char> span = stackalloc char[8];
    hexcode[0..2].CopyTo(span[0..2]);
    hexcode[2..4].CopyTo(span[2..4]);
    hexcode[4..6].CopyTo(span[4..6]);
    span[6] = span[7] = 'F';
    return ParseFromSpan8(span);
}

Color ParseFrom8(string hexcode)
{
    var span = hexcode.AsSpan();
    return ParseFromSpan8(span);
}

Color ParseFromSpan8(in ReadOnlySpan<char> span)
{
    return new Color
    {
        R = byte.Parse(span[0..2], NumberStyles.HexNumber),
        G = byte.Parse(span[2..4], NumberStyles.HexNumber),
        B = byte.Parse(span[4..6], NumberStyles.HexNumber),
    };
}






















void Write(string text, in Color color)
{
    var length = text.Length + 6 + 10 + 3 + DigitCount(color.R) + DigitCount(color.G) + DigitCount(color.B);
    var buffer = string.Create(length, (color, text: text.AsMemory()), (span, data) =>
    {
        span[0] = '\u001b';
        span[1] = '[';
        span[2] = '3';
        span[3] = '8';
        span[4] = ';';
        span[5] = '2';
        span[6] = ';';

        var colorSpan = data.color.ToColorSpan();
        var i = 7;
        colorSpan.R.CopyTo(span[i..(i + colorSpan.R.Length)]);
        i += colorSpan.R.Length;
        span[i++] = ';';
        colorSpan.G.CopyTo(span[i..(i + colorSpan.G.Length)]);
        i += colorSpan.G.Length;
        span[i++] = ';';
        colorSpan.B.CopyTo(span[i..(i + colorSpan.B.Length)]);
        i += colorSpan.B.Length;
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

static int DigitCount(byte channel) => channel switch
{
    <= 9 => 1,
    <= 99 => 2,
    _ => 3
};

readonly struct Color
{
    public required byte R { get; init; }
    public required byte G { get; init; }
    public required byte B { get; init; }
    public byte A { get; init; }
    public Color() => A = 255;

    public ColorSpan ToColorSpan()
    {
        return new()
        {
            R = R.ToString().AsSpan(),
            G = G.ToString().AsSpan(),
            B = B.ToString().AsSpan(),
        };
    }
}

readonly ref struct ColorSpan
{
    public required ReadOnlySpan<char> R { get; init; }
    public required ReadOnlySpan<char> G { get; init; }
    public required ReadOnlySpan<char> B { get; init; }
}
