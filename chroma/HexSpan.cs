namespace Chroma;

public readonly ref struct HexSpan
{
    public ReadOnlySpan<char> R { get; }
    public ReadOnlySpan<char> G { get; }
    public ReadOnlySpan<char> B { get; }
    public ReadOnlySpan<char> Css { get; }

    public HexSpan(Color color)
    {
        Css = string.Create(7, color, (span, color) =>
        {
            span[0] = '#';
            FillHexDigits(color.R, span[1..3]);
            FillHexDigits(color.G, span[3..5]);
            FillHexDigits(color.B, span[5..7]);
        });

        R = Css[1..3];
        G = Css[3..5];
        B = Css[5..7];
    }

    static char GetHexDigitFromLowerFourBits(byte bits) => bits switch
    {
        0 => '0',
        1 => '1',
        2 => '2',
        3 => '3',
        4 => '4',
        5 => '5',
        6 => '6',
        7 => '7',
        8 => '8',
        9 => '9',
        10 => 'A',
        11 => 'B',
        12 => 'C',
        13 => 'D',
        14 => 'E',
        _ => 'F',
    };

    static void FillHexDigits(byte value, in Span<char> slice)
    {
        var left = (byte)((0b1111_0000 & value) >> 4);
        var right = (byte)(0b0000_1111 & value);

        slice[0] = GetHexDigitFromLowerFourBits(left);
        slice[1] = GetHexDigitFromLowerFourBits(right);
    }
}
