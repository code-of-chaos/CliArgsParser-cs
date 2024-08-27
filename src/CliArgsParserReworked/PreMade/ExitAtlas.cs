// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParserReworked.Contracts;
using CliArgsParserReworked.Contracts.Attributes;

namespace CliArgsParserReworked.PreMade;

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
