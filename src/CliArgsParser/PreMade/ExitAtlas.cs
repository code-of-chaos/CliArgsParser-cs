// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Contracts;
using CliArgsParser.Contracts.Attributes;

namespace CliArgsParser.PreMade;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The ExitAtlas class is a command atlas that provides a method for exiting the CLI application.
/// </summary>
public class ExitAtlas(ICliParser parser) : ICommandAtlas {
    /// <summary>
    /// Represents a command that exits the CLI application.
    /// </summary>
    [Command("exit")]
    [Description("Exits the CLI application.")]
    public void CommandExit() {
        parser.IsAlive = false;
    }
}
