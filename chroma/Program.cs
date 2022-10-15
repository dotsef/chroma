using System.Diagnostics;
using System.Reflection;

using Chroma;

var exit = args switch
{
    [..] when args.Any(a => a is "--version" or "-v") => VersionCommand(),
    [var hexcode] => ParseColorCommand(hexcode),
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

int ParseColorCommand(in ReadOnlySpan<char> hexcode)
{
    if (!Color.TryParse(hexcode, null, out var color))
    {
        Terminal.WriteLine($"'{hexcode}' is not a parsable hex-formatted color", Color.Red);
        return -1;
    }

    var rgb = new RGBSpan(color);
    Terminal.WriteLine(rgb.Css.ToString(), color);
    var hsl = new HSLSpan(color);
    Terminal.WriteLine(hsl.Css.ToString(), color);
    return 0;
}

int UsageCommand()
{
    Terminal.WriteLine("Please provide a single color in hex format.", Color.Red);
    return -1;
}