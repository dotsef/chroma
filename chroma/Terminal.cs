namespace Chroma;

public static class Terminal
{
    public static void Write(ref ColoredInterpolatedStringHandler handler)
        => Console.Out.Write(handler.ToStringAndClear());

    public static void Write<T>(T value)
        => Write($"{value}");

    public static void Write(in ReadOnlySpan<char> span)
        => Console.Out.Write(span);

    public static void WriteLine(ref ColoredInterpolatedStringHandler handler)
        => Console.Out.WriteLine(handler.ToStringAndClear());

    public static void WriteLine<T>(T value)
        => WriteLine($"{value}");

    public static void WriteLine(in ReadOnlySpan<char> span)
        => Console.Out.WriteLine(span);

    public static void Write(ref ColoredInterpolatedStringHandler handler, in Color color)
        => Console.Out.Write(handler.ToStringAndClear().Colorize(color));

    public static void Write<T>(T value, in Color color)
        => Write($"{value}", color);

    public static void Write(in ReadOnlySpan<char> span, in Color color)
        => Write($"{span}", color);

    public static void WriteLine(ref ColoredInterpolatedStringHandler handler, in Color color)
        => Console.Out.WriteLine(handler.ToStringAndClear().Colorize(color));

    public static void WriteLine<T>(T value, in Color color)
       => WriteLine($"{value}", color);

    public static void WriteLine(in ReadOnlySpan<char> span, in Color color)
        => WriteLine($"{span}", color);

    public static string Colorize(this string text, in Color textColor, Color? backgroundColor = null)
    {
        var colorized = $"\u001b[38;2;{textColor.R};{textColor.G};{textColor.B}m{text}\u001b[0m";

        return backgroundColor is not null
            ? $"\u001b;48;2;{backgroundColor.Value.R};{backgroundColor.Value.G};{backgroundColor.Value.B}m{colorized}\u001b[0m"
            : colorized;
    }
}
