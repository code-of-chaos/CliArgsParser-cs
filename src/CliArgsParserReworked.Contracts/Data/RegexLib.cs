// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text.RegularExpressions;

namespace CliArgsParserReworked.Contracts.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static partial class RegexLib {
    [GeneratedRegex("([A-Z]+(?=[A-Z0-9][a-z0-9])|[A-Z0-9][a-z0-9]*)", RegexOptions.Compiled)]
    private static partial Regex RegexSplitCamelCase();
    public static readonly Regex SplitCamelCase = RegexSplitCamelCase();
}
