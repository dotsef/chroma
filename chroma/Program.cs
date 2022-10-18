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
    Terminal.WriteLine(rgb.Css, parsedColor, 2);
    var hsl = new HSLSpan(parsedColor);
    Terminal.WriteLine(hsl.Css, parsedColor, 2);
    var hex = new HexSpan(parsedColor);
    Terminal.WriteLine(hex.Css, parsedColor, 2);
    return 0;
}

int UsageCommand()
{
    Terminal.WriteLine("Please provide a single color in hex format.", Color.Red);
    Console.WriteLine();
    Terminal.WriteLine("Usage: chroma [color]");
    return -1;
}