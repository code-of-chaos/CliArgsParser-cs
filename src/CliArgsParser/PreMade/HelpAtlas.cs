// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Contracts;
using CliArgsParser.Contracts.Attributes;
using CliArgsParser.Contracts.Types;
using System.Reflection;
using System.Text;

namespace CliArgsParser.PreMade;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// HelpAtlas is a class that provides help text for commands in a command-line application.
/// </summary>
/// <remarks>
/// HelpAtlas implements the ICommandAtlas interface and is meant to be used in conjunction with a command-line argument parser (ICliArgsParser).
/// It provides a command called "help" that prints help text for all commands or a specific command.
/// </remarks>
public class HelpAtlas(ICliArgsParser parser) : ICommandAtlas {
    /// <summary>
    /// Represents a method in the CommandAtlas class that provides help information for commands.
    /// </summary>
    /// <param name="args">An instance of the <see cref="HelpArgs"/> class that contains the command name and expand flag.</param>
    [Command<HelpArgs>("help")]
    [Description("Prints this help text")]
    public void CommandHelp(HelpArgs args) {
        if (args.Expand) PrintAllCommandsExpanded();
        else if (string.IsNullOrEmpty(args.Name)) PrintAllCommands();
        else PrintCommandArguments(args.Name);
    }

    #region Helper Methods
    private void PrintCommandArguments(string commandName) {
        var sb = new StringBuilder();
        
        foreach ((string namedArg, string description) in GetCommandArguments(commandName)) {
            sb.AppendLine($"- {namedArg} : {description}");
        }
        
        Console.WriteLine(sb.ToString());
    }


    private Dictionary<(MethodInfo, ICommandAtlas), List<(string Key, string Description)>> GetAllCommands() {
        // ReSharper disable once SuggestVarOrType_Elsewhere
        var enumerable = parser.Commands
                .OrderBy(pair => pair.Value.CommandAtlas.GetType().Name)
                .ThenBy(pair => pair.Key)
            ;

        Dictionary<(MethodInfo, ICommandAtlas), List<(string Key, string Description)>> groupedCommands = new();
        foreach ((string? key, CommandMethodInfo info ) in enumerable) {
            (MethodInfo methodInfo, ICommandAtlas atlas) commandKey = (info.Info, info.CommandAtlas);
            if (!groupedCommands.TryGetValue(commandKey, out List<(string Key, string Description)>? commandsList)) {
                commandsList = (List<(string Key, string Description)>) [];
                groupedCommands[commandKey] = commandsList;
            }
            commandsList.Add((key, info.DescriptionAttribute?.Description ?? string.Empty));
        }
        return groupedCommands;
    }

    private IEnumerable<(string namedArg, string description)> GetCommandArguments(string commandName) {
        // Find command by name
        KeyValuePair<string, CommandMethodInfo> command = parser.Commands.FirstOrDefault(c => c.Key == commandName);

        // ReSharper disable once SuggestVarOrType_Elsewhere
        var enumerable = command.Value.ParameterType.GetProperties()
            .Select(prop => (
                prop,
                namedArg: (IAttributeWithName?)prop.GetCustomAttribute<ArgValueAttribute>() ?? prop.GetCustomAttribute<ArgFlagAttribute>() ,
                description: prop.GetCustomAttribute<DescriptionAttribute>()
            ))
            .Where(tuple => tuple.namedArg != null)
            .Select(tuple => (tuple.namedArg!.Name , tuple.description?.Description ?? string.Empty))
        ;

        return enumerable;
    }
    
    private void PrintAllCommands() {
        var sb = new StringBuilder();
        
        // ReSharper disable once SuggestVarOrType_Elsewhere
        var groupedCommands = GetAllCommands(); 
        int maxKeyCount = groupedCommands.Values
            .Select(list => string.Join(", ", list.OrderByDescending(cmd => cmd.Key.Length).Select(cmd => cmd.Key)).Length)
            .Max() + 1;

        foreach (KeyValuePair<(MethodInfo, ICommandAtlas), List<(string Key, string Description)>> commandGroup in groupedCommands) {
            List<(string Key, string Description)> sortedCommands = commandGroup.Value.OrderByDescending(cmd => cmd.Key.Length).ToList();
            string allCommandNames = string.Join(", ", sortedCommands.Select(cmd => cmd.Key));
            string description = commandGroup.Value.First().Description; // Use the first description as they should be the same
            sb.AppendLine($"{allCommandNames.PadRight(maxKeyCount, ' ')}| {description}");
        }
        
        Console.WriteLine(sb.ToString());
    }
    
    private void PrintAllCommandsExpanded() {
        var sb = new StringBuilder();
        
        // ReSharper disable once SuggestVarOrType_Elsewhere
        var groupedCommands = GetAllCommands(); 
        int maxKeyCount = groupedCommands.Values
            .Select(list => string.Join(", ", list.OrderByDescending(cmd => cmd.Key.Length).Select(cmd => cmd.Key)).Length)
            .Max() + 1;
        
        foreach (KeyValuePair<(MethodInfo, ICommandAtlas), List<(string Key, string Description)>> commandGroup in groupedCommands) {
            List<(string Key, string Description)> sortedCommands = commandGroup.Value.OrderByDescending(cmd => cmd.Key.Length).ToList();
            string firstCommand = sortedCommands.First().Key;
            string allCommandNames = string.Join(", ", sortedCommands.Select(cmd => cmd.Key));
            string description = commandGroup.Value.First().Description; // Use the first description as they should be the same
            sb.AppendLine($"{allCommandNames.PadRight(maxKeyCount, ' ')}| {description}");
            
            foreach ((string namedArg, string varDesc) in GetCommandArguments(firstCommand)) {
                string name = $"    - {namedArg}";
                sb.AppendLine($"{name.PadRight(maxKeyCount, ' ')}| {varDesc}");
            }
            
            sb.AppendLine();
        }
        
        Console.WriteLine(sb.ToString());
        
    }
    #endregion
}
