// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Contracts;
using CliArgsParser.Contracts.Attributes;

namespace CliArgsParser.PreMade;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ExitAtlas(ICliParser parser) : ICommandAtlas {
    [Command("exit")]
    [Description("Exits the CLI application.")]
    public void CommandExit() {
        parser.IsAlive = false;
    }
}
