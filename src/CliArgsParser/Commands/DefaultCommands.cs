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
/// <summary>
/// Represents a set of default commands.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")] // disables the annoying "class is not used"
public class DefaultCommands : ICliCommandAtlas {
    /// <summary>
    /// Callback method for the "help" command. Displays all available commands and their descriptions.
    /// </summary>
    /// <param name="noArgs">NoArgs instance</param>
    /// <returns>true if the command was executed successfully, false otherwise</returns>
    [CliCommand<NoArgs>("help", "Display all commands.")]
    public bool CallbackHelp(NoArgs noArgs) {
        int maxCommandNameLength = Math.Max(
            CliArgsParser.Descriptions.Keys.Select(k => k.Length).Max(), 
            12 // always have at least 12 as padding value
        ); 
        string title = "Command Name".PadRight(maxCommandNameLength);
        var pattern = new string('-', maxCommandNameLength);

        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"{title} | Description");
        stringBuilder.AppendLine($"{pattern}-|------------");
            
        foreach ((string name, string? desc) in CliArgsParser.Descriptions) {
            stringBuilder.AppendLine($"{name.PadRight(maxCommandNameLength)} | {desc}");
        }
            
        Console.WriteLine(stringBuilder);
        return true;
    }

    /// <summary>
    /// Executes the exit command.
    /// </summary>
    /// <param name="forceArgs">The force arguments.</param>
    /// <returns>Returns false if the program should exit; otherwise, returns true.</returns>
    [CliCommand<ForceArgs>("exit", "Exit the program whilst in user input parsing mode. Will not work if in 'no breaking mode' without -f or --force")]
    public bool CallbackExit(ForceArgs forceArgs) {
        if (forceArgs is { Force: true, Verbose: false }) Environment.Exit(-1);
        if (forceArgs is { Force: true, Verbose: true }) throw new Exception("Forced an exit");
        
        Environment.Exit(0);
        return false;
    }
}