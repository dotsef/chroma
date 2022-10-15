namespace Chroma;

public static class Utils
{
    public static int DigitCount(this byte channel) => channel switch
    {
        <= 9 => 1,
        <= 99 => 2,
        _ => 3
    };
}