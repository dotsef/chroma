namespace Chroma;

public static class TerminalExtensions
{
    public static string Colorize<T>(this T value, in Color textColor)
        => $"\u001b[38;2;{textColor.R};{textColor.G};{textColor.B}m{value}\u001b[0m";

    public static string Colorize(this ReadOnlySpan<char> value, in Color textColor)
        => $"\u001b[38;2;{textColor.R};{textColor.G};{textColor.B}m{value}\u001b[0m";
}
