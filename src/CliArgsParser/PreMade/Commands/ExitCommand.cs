// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Attributes;
using JetBrains.Annotations;

namespace CliArgsParser.PreMade.Commands;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// A standard atlas with commands that will exit your application.
/// </summary>
[CommandAtlas]
[UsedImplicitly]
public class ExitCommand {
    /// <summary>
    /// Represents a command that provides help information.
    /// </summary>
    /// <remarks>
    /// This method is used to display help information for the CLI application.
    /// It prints out the available commands and their descriptions.
    /// </remarks>
    [Command("exit")]
    [UsedImplicitly]
    public void CommmandHelp() {
        Environment.Exit(-1);
    }
}
