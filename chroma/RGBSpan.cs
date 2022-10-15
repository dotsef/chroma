namespace Chroma;

readonly ref struct RGBSpan
{
    public ReadOnlySpan<char> R { get; }
    public ReadOnlySpan<char> G { get; }
    public ReadOnlySpan<char> B { get; }
    public ReadOnlySpan<char> Css { get; }

    public RGBSpan(Color color)
    {
        Css = $"rgb({color.R}, {color.G}, {color.B})";
        var r0 = 4;
        var r1 = r0 + color.R.DigitCount();
        R = Css[r0..r1];
        var g0 = r1 + 2;
        var g1 = g0 + color.G.DigitCount();
        G = Css[g0..g1];
        var b0 = g1 + 2;
        var b1 = b0 + color.B.DigitCount();
        B = Css[b0..b1];
    }
}

public readonly ref struct HSLSpan
{
    public ReadOnlySpan<char> H { get; }
    public ReadOnlySpan<char> S { get; }
    public ReadOnlySpan<char> L { get; }
    public ReadOnlySpan<char> Css { get; }

    public HSLSpan(Color color)
    {
        var (h, s, l) = ToHSL(color);
        var hue = h.ToString("0.00");
        var saturation = (s * 100d).ToString("0.00");
        var lightness = (l * 100d).ToString("0.00");
        Css = $"hsl({hue} {saturation}% {lightness}%)";
        var h0 = 4;
        var h1 = h0 + hue.Length;
        H = Css[h0..h1];
        var s0 = h1 + 1;
        var s1 = s0 + saturation.Length;
        S = Css[s0..s1];
        var l0 = s1 + 2;
        var l1 = l0 + lightness.Length;
        L = Css[l0..l1];
    }

    private static (double hue, double saturation, double lightness) ToHSL(in Color color)
    {
        var max = Math.Max(color.R, Math.Max(color.G, color.B));
        var min = Math.Min(color.R, Math.Min(color.G, color.B));
        var diff = (max - min) / 255d;

        var lightness = (max + min) / 510d;

        var saturation = lightness is 0d
            ? 0d
            : diff / (1d - (2d * lightness - 1d));

        var denom = Math.Sqrt(color.R * color.R + color.G * color.G + color.B * color.B - color.R * color.G - color.R * color.B - color.G * color.B);
        var prehue = Math.Acos((color.R - color.G / 2d - color.B / 2d) / denom);
        var hue = color.G >= color.B
            ? prehue
            : 360d - prehue;

        return (hue, saturation, lightness);
    }
}