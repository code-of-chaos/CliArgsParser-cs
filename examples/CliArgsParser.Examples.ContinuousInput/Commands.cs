// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Attributes;
using CliArgsParser.Commands;

namespace CliArgsParser.Examples.ContinuousInput;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Commands : CliCommandAtlas {
    [CliCommand<NoArgs>("test")]
    public bool CallbackTest(NoArgs _) {
        Console.WriteLine("Testing this");
        
        return true;
    }
}