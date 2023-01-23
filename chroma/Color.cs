using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Chroma;

public readonly struct Color : ISpanParsable<Color>
{
    public required byte R { get; init; }
    public required byte G { get; init; }
    public required byte B { get; init; }
    public byte A { get; init; }
    public Color() => A = 255;

    private const NumberStyles Hex = NumberStyles.HexNumber;

    private static readonly char[] _trimStart = new char[]
    {
        ' ', '('
    };

    private static readonly char[] _trimEnd = new char[]
    {
        ' ', ')'
    };

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Color result)
    {
        if (s is ['r', 'g', 'b', 'a', .. var rgbaParts])
            return TryParseRgba(rgbaParts.TrimStart(_trimStart).TrimEnd(_trimEnd), provider, out result);

        if (s is ['r', 'g', 'b', .. var rgbParts])
            return TryParseRgb(rgbParts.TrimStart(_trimStart).TrimEnd(_trimEnd), provider, out result);

        if (s is ['h', 's', 'l', .. var hslParts])
            return TryParseHsl(hslParts.TrimStart(_trimStart).TrimEnd(_trimEnd), provider, out result);

        if (s.Length > 0 && s[0] is '#')
            s = s[1..];

        return s.Length switch
        {
            3 /* RGB      */ => TryParse3(s, provider, out result),
            4 /* RGBA     */ => TryParse4(s, provider, out result),
            6 /* RRGGBB   */ => TryParse6(s, provider, out result),
            8 /* RRGGBBAA */ => TryParse8(s, provider, out result),
            _ => Failure(out result),
        };

        bool TryParseRgb(ReadOnlySpan<char> s, IFormatProvider? provider, out Color result)
        {
            var i = 0;
            var j = 0;

            RgbParseNext(ref i, ref j, s, out var r);
            RgbParseNext(ref i, ref j, s, out var g);
            RgbParseNext(ref i, ref j, s, out var b);

            result = new Color { R = r, G = g, B = b };
            return true;
        }

        bool TryParseRgba(ReadOnlySpan<char> s, IFormatProvider? provider, out Color result)
        {
            var i = 0;
            var j = 0;

            RgbParseNext(ref i, ref j, s, out var r);
            RgbParseNext(ref i, ref j, s, out var g);
            RgbParseNext(ref i, ref j, s, out var b);
            RgbParseNext(ref i, ref j, s, out var a);

            result = new Color { R = r, G = g, B = b, A = a };
            return true;
        }

        bool RgbParseNext(ref int i, ref int j, ReadOnlySpan<char> s, out byte part)
        {
            while (i < s.Length && char.IsDigit(s[i])) { i++; }

            if (!byte.TryParse(s[j..i], provider, out part))
            {
                part = default;
                return false;
            }

            do { i++; } while (i < s.Length && (char.IsWhiteSpace(s[i]) || s[i] is ','));

            j = i;

            return true;
        }

        bool TryParseHsl(ReadOnlySpan<char> s, IFormatProvider? provider, out Color result)
        {
            result = default;
            return false;
        }

        bool TryParse3(ReadOnlySpan<char> s, IFormatProvider? provider, out Color result)
        {
            Span<char> span = stackalloc char[8];
            span[0] = span[1] = s[0];
            span[2] = span[3] = s[1];
            span[4] = span[5] = s[2];
            span[6] = span[7] = 'F';
            return TryParse8(span, provider, out result);
        }

        bool TryParse4(ReadOnlySpan<char> s, IFormatProvider? provider, out Color result)
        {
            Span<char> span = stackalloc char[8];
            span[0] = span[1] = s[0];
            span[2] = span[3] = s[1];
            span[4] = span[5] = s[2];
            span[6] = span[7] = s[3];
            return TryParse8(span, provider, out result);
        }

        bool TryParse6(ReadOnlySpan<char> s, IFormatProvider? provider, out Color result)
        {
            Span<char> span = stackalloc char[8];
            s[0..2].CopyTo(span[0..2]);
            s[2..4].CopyTo(span[2..4]);
            s[4..6].CopyTo(span[4..6]);
            span[6] = span[7] = 'F';
            return TryParse8(span, provider, out result);
        }

        bool TryParse8(ReadOnlySpan<char> s, IFormatProvider? provider, out Color result)
        {
            if (byte.TryParse(s[0..2], Hex, provider, out var r)
             && byte.TryParse(s[2..4], Hex, provider, out var g)
             && byte.TryParse(s[4..6], Hex, provider, out var b)
             && byte.TryParse(s[6..8], Hex, provider, out var a))
            {
                result = new Color { R = r, G = g, B = b, A = a };
                return true;
            }

            result = default;
            return false;
        }

        bool Failure(out Color result)
        {
            result = default;
            return false;
        }
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Color result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public static Color Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => TryParse(s, provider, out var color)
            ? color
            : throw new InvalidOperationException($"Failed to parse '{s}' to a color");

    public static Color Parse(string s, IFormatProvider? provider) => Parse(s.AsSpan(), provider);

    public static Color Random
    {
        get
        {
            Span<byte> rgb = stackalloc byte[4];
            System.Random.Shared.NextBytes(rgb);
            return new Color
            {
                R = rgb[0],
                G = rgb[1],
                B = rgb[2],
                A = rgb[3],
            };
        }
    }

    public static readonly Color Red = new() { R = 255, G = 0, B = 0 };

    public static readonly Color White = new() { R = 255, G = 255, B = 255 };
}
