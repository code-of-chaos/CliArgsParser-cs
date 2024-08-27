// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable CheckNamespace
namespace CliArgsParser;
// ReSharper restore CheckNamespace

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The HelpArgs class is used by the pre-made <see cref="HelpAtlas.CommandHelp"/> for its arguments.
/// </summary>
public class HelpArgs : ICommandParameters {
    /// <summary>
    /// Represents a property in the HelpArgs class that stores the specific command for which to get help.
    /// </summary>
    [ArgValue("name")] [Description("The specific command to get help for")] public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Represents a property in the HelpArgs class that indicates whether to expand the list of commands and their arguments.
    /// </summary>
    /// <value>
    /// <c>true</c> if the list of commands and their arguments should be expanded; otherwise, <c>false</c>.
    /// The default value is <c>false</c>.
    /// </value>
    [ArgFlag("expand")] [Description("Returns a full list of all commands and their arguments")] public bool Expand { get; set; } = false;
}
