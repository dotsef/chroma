namespace Chroma;

readonly ref struct RGBSpan
{
    public ReadOnlySpan<char> R { get; }
    public ReadOnlySpan<char> G { get; }
    public ReadOnlySpan<char> B { get; }
    public ReadOnlySpan<char> A { get; }
    public ReadOnlySpan<char> Css { get; }

    public RGBSpan(Color color)
    {
        var str = $"rgba({color.R}, {color.G}, {color.B}, {color.A})";
        var span = str.AsSpan();
        var r0 = 5;
        var r1 = r0 + color.R.DigitCount();
        R = span[r0..r1];
        var g0 = r1 + 2;
        var g1 = g0 + color.G.DigitCount();
        G = span[g0..g1];
        var b0 = g1 + 2;
        var b1 = b0 + color.B.DigitCount();
        B = span[b0..b1];
        var a0 = b1 + 2;
        var a1 = a0 + color.A.DigitCount();
        A = span[a0..a1];
    }
}
