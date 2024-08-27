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
    
    [GeneratedRegex("""(?:--|-)(\w+)(?:=(".*?"|\S+))?""", RegexOptions.Compiled)] 
    private static partial Regex RegexArgs();
    public static readonly Regex Args = RegexArgs();

    [GeneratedRegex("""&&(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)""", RegexOptions.Compiled)]
    private static partial Regex RegexSplitCommands();
    private static readonly Regex SplitCommands = RegexSplitCommands();
}
