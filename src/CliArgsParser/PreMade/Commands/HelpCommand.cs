// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Attributes;

namespace CliArgsParser.PreMade.Commands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// A standard atlas with commands that will show some help information.
/// </summary>
[CommandAtlas]
public class HelpCommand() {
    /// <summary>
    /// Represents a help command.
    /// </summary>
    /// <remarks>
    /// The help command provides information about available commands and their descriptions.
    /// </remarks>
    [Command("help")]
    public void CommmandHelp() {
        Console.WriteLine("There is no help");
    }
}