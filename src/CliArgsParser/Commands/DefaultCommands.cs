// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CliArgsParser.Attributes;
using CliArgsParser.Contracts;

namespace CliArgsParser.Commands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[SuppressMessage("ReSharper", "UnusedMember.Global")] // disables the annoying "class is not used"
public class DefaultCommands : ICliCommandAtlas {

    [CliCommand<NoArgs>("help", "Display all commands.")]
    public bool CallbackHelp(NoArgs _) {
        int maxCommandNameLength = Math.Max(
            CliArgsParser.Descriptions.Keys.Select(k => k.Length).Max(), 
            12 // always have at least 12 as padding value
        ); 
        string title = "Command Name".PadRight(maxCommandNameLength);
        string pattern = new string('-', maxCommandNameLength);

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"{title} | Description");
        stringBuilder.AppendLine($"{pattern}-|------------");
            
        foreach ((string name, string? desc) in CliArgsParser.Descriptions) {
            stringBuilder.AppendLine($"{name.PadRight(maxCommandNameLength)} | {desc}");
        }
            
        Console.WriteLine(stringBuilder);
        return true;
    }
    
    [CliCommand<ForceArgs>("exit", "Exit the program whilst in user input parsing mode. Will not work if in 'no breaking mode' without -f or --force")]
    public bool CallbackExit(ForceArgs forceArgs) {
        if (forceArgs is { Force: true, Verbose: false }) Environment.Exit(-1);
        if (forceArgs is { Force: true, Verbose: true }) throw new Exception("Forced an exit");
        
        Environment.Exit(0);
        return false;
    }
}