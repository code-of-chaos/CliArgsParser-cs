// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using CliArgsParser.Attributes;

namespace CliArgsParser.Examples.RegisterAtlas;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CommandsStatic : CliCommandAtlas {
    
    [CliCommand<ArgOptions>("test")]
    public void CallbackTest(ArgOptions argOptions) {
        Console.WriteLine(argOptions.Username != null ? 
            $"Hello, {argOptions.Username}!" : 
            "Hello, stranger"
        );
    }
    
}