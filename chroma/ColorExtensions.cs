namespace Chroma;

public static class ColorExtensions
{
    public static (double hue, double saturation, double lightness) ToHSL(this Color color)
    {
        var r = color.R / 255d;
        var g = color.G / 255d;
        var b = color.B / 255d;

        var min = Math.Min(r, Math.Min(g, b));
        var max = Math.Max(r, Math.Max(g, b));
        var diff = max - min;

        var l = (max + min) / 2d;

        var s = diff is 0d
            ? 0d
            : diff / (1d - Math.Abs(2 * l - 1d));

        var h = diff is 0d
            ? 0d
            : max == r
            ? 60d * ((g - b) / diff % 6d)
            : max == g
            ? 60d * (((b - r) / diff) + 2d)
            : 60d * (((r - g) / diff) + 4d);

        while (h < 0)
            h += 360d;

        return (h, s, l);
    }

    public static (byte r, byte g, byte b) ToRGB(this (double h, double s, double l) hsl)
    {
        var (h, s, l) = hsl;

        var c = s * (1d - Math.Abs(2d * l - 1d));
        var x = c * (1d - Math.Abs(h / 60d % 2d - 1d));
        var m = l - c / 2d;
        var (r, g, b) = h switch
        {
            >= 0d and < 60d => (c, x, 0d),
            >= 60d and < 120d => (x, c, 0d),
            >= 120d and < 180d => (0d, c, x),
            >= 180d and < 240d => (0d, x, c),
            >= 240d and < 300d => (x, 0d, c),
            >= 300d and <= 360d => (c, 0d, x),
            _ => throw new ArgumentOutOfRangeException(nameof(h), "The given hue was outside the allowed range of [0, 360]")
        };
        r += m;
        g += m;
        b += m;
        return ((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
    }
}