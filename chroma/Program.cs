using System.Diagnostics;
using System.Reflection;

using Chroma;

var exit = args switch
{
    [..] when args.Any(a => a is "--version" or "-v") => VersionCommand(),
    [..] when args.Any(a => a is "--help" or "-h") => UsageCommand(),
    [..] => ParseColorCommand(string.Join(" ", args)),
};

return exit;

int VersionCommand()
{
    var version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
    Debug.Assert(version.ProductVersion is not null, "The version should be known at runtime");
    Console.WriteLine(version.ProductVersion.Colorize(Color.Random));
    return 0;
}

int ParseColorCommand(in ReadOnlySpan<char> input)
{
    if (!Color.TryParse(input, null, out var parsedColor))
        return ErrorCommand(input);

    var (h, s, l) = parsedColor.ToHSL();

    Console.Write($"""

          rgb({parsedColor.R}, {parsedColor.G}, {parsedColor.B})
          hsl({h:N0} {s * 100d:N0}% {l * 100d:N0}%)
          #{parsedColor.R:X2}{parsedColor.G:X2}{parsedColor.B:X2}

        """.Colorize(parsedColor));

    return 0;
}

int UsageCommand()
{
    Console.Write($"""
        Convert between color formats by providing a color in any format

            Usage: chroma [color]

        """);

    return 0;
}

int ErrorCommand(in ReadOnlySpan<char> input)
{
    Console.WriteLine($"'{input.Colorize(Color.Red)}' is not a recognized command or parsable color");
    return -1;
}