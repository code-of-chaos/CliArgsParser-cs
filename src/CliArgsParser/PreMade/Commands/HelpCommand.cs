// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using CliArgsParser.Attributes;
using CliArgsParser.Contracts;
using CliArgsParser.PreMade.Args;

namespace CliArgsParser.PreMade.Commands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[CommandAtlas]
public class HelpCommand() {

    [Command("help")]
    public void CommmandHelp() {
        Console.WriteLine("IT WORKS!");
    }
    
    [Command<ForceArgs>("help")]
    public void CommmandHelp(ForceArgs args) {
        Console.WriteLine($"IT WORKS! and is forced: {args.IsForced}");
    }
}