using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Chroma;

readonly struct Color : ISpanParsable<Color>
{
    public required byte R { get; init; }
    public required byte G { get; init; }
    public required byte B { get; init; }
    public byte A { get; init; }
    public Color() => A = 255;

    private const NumberStyles Hex = NumberStyles.HexNumber;

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Color result)
    {
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

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Color result)
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
}
