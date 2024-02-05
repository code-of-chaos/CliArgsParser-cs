// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using CliArgsParser.Attributes;
using CliArgsParser.Commands;

namespace CliArgsParser.Examples.ContinuousInput;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class Commands : CliCommandAtlas {
    [CliCommand<NoArgs>("test")]
    public bool CallbackTest(NoArgs _) {
        Console.WriteLine("Testing this");
        return true;
    }
}