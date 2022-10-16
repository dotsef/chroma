namespace Chroma;

public readonly ref struct RGBSpan
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
