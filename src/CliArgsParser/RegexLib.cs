// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text.RegularExpressions;

namespace CliArgsParser;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The `RegexLib` class contains a collection of regular expressions used in the CliArgsParser library.
/// It provides pre-compiled regular expressions for common operations such as splitting camel case strings, parsing command line arguments, and splitting command strings.
/// This class is static and cannot be instantiated.
/// </summary>
public static partial class RegexLib {
    /// <summary>
    /// Matches are used to split a string on CamelCase segments.
    /// </summary>
    public static readonly Regex SplitCamelCase = RegexSplitCamelCase();

    [GeneratedRegex("([A-Z]+(?=[A-Z0-9][a-z0-9])|[A-Z0-9][a-z0-9]*)", RegexOptions.Compiled)] private static partial Regex RegexSplitCamelCase();

    /// <summary>
    /// Matches command-line options in the format of <c>--option=value</c> or <c>-o value</c>.
    /// </summary>
    public static readonly Regex Args = RegexArgs();

    [GeneratedRegex("""(?:--|-)(\w+)(?:=(".*?"|\S+))?""", RegexOptions.Compiled)] private static partial Regex RegexArgs();

    /// <summary>
    /// Split a string containing multiple commands separated by "&amp;&amp;" into an array of individual commands.
    /// </summary>
    /// <returns>A regular expression pattern to split the commands.</returns>
    public static readonly Regex SplitCommands = RegexSplitCommands();

    [GeneratedRegex("""&&(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)""", RegexOptions.Compiled)] private static partial Regex RegexSplitCommands();
}
