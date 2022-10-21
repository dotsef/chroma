using System.Diagnostics;
using System.Reflection;

using Chroma;

var exit = args switch
{
    [..] when args.Any(a => a is "--version" or "-v") => VersionCommand(),
    [var color] => ParseColorCommand(color),
    _ => UsageCommand(),
};

return exit;

int VersionCommand()
{
    var version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
    Debug.Assert(version.ProductVersion is not null, "The version should be known at runtime");
    Terminal.WriteLine(version.ProductVersion, Color.Random);
    return 0;
}

int ParseColorCommand(in ReadOnlySpan<char> color)
{
    if (!Color.TryParse(color, null, out var parsedColor))
    {
        Terminal.WriteLine($"'{color}' is not a parsable color", Color.Red);
        return -1;
    }

    var rgb = new RGBSpan(parsedColor);
    Terminal.WriteLine(rgb.Css, parsedColor);
    var hsl = new HSLSpan(parsedColor);
    Terminal.WriteLine(hsl.Css, parsedColor);
    var hex = new HexSpan(parsedColor);
    Terminal.WriteLine(hex.Css, parsedColor);
    return 0;
}

int UsageCommand()
{
    Terminal.Write($"""
        {"Please provide a single color in hex, rgb, or hsl format.":F00}

            Usage: chroma [color]

        """);

    return -1;
}