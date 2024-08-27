// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Contracts;
using CliArgsParser.Contracts.Attributes;

namespace CliArgsParser.PreMade;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class HelpArgs : IParameters {
    [ArgValue("name"), Description("The specific command to get help for")] public string Name { get; set; } = string.Empty;
    [ArgFlag("expand"), Description("Returns a full list of all commands and their arguments")] public bool Expand { get; set; } = false;
}
