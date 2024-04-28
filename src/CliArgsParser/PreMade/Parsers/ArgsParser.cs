// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Data;
using CliArgsParser.Contracts;
using CliArgsParser.Contracts.Common;
using CliArgsParser.PreMade.Args;

namespace CliArgsParser.PreMade.Parsers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ArgsParser : AbstractParser {
    public override bool TryParse<T>(string input, out T? output) where T : default {
        output = default;

        foreach (string commandString in GetCommands(input)) {
            if (!TryGetCommand(commandString, out string? commandName, out Dictionary<string, string>? args)) {
                return false;
            }

            if (!CommandStructs.TryGetValue(commandName, out CommandRecord? commandRecord)) {
                return false;
            }

            try {
                var parameters = commandRecord.ParameterParser.Parse(args);
                
                if(parameters?.GetType() == typeof(NoArgs)) {
                    output = (T)commandRecord.Delegate.DynamicInvoke()!;
                    return true;
                }
                output = (T)commandRecord.Delegate.DynamicInvoke(parameters)!;
                return true;
            }
            catch (Exception e){
                
                Log.Error(e, "error");
                Console.WriteLine(e);
                
                return false;
            }
        }

        return true;
    }

    public override async Task<bool> TryParseAsync(string input) {
        Log.Debug("Starting async command parsing.");

        var tasks = GetCommands(input).Select(commandString => ProcessCommandStringAsync(commandString));

        await Task.WhenAll(tasks);

        Log.Debug("Finished async command parsing.");
        return true;
    }
    
    private async Task ProcessCommandStringAsync(string commandString) {
        Log.Debug($"Processing command: {commandString}");

        if (!TryGetCommand(commandString, out string? commandName, out Dictionary<string, string>? args)) {
            Log.Warning($"Failed to get command for string: {commandString}");
            return;
        }

        if (!CommandStructs.TryGetValue(commandName, out CommandRecord? commandRecord)) {
            Log.Warning($"No command record found for command name: {commandName}");
            return;
        }

        try {
            Log.Debug("Parsing parameters for command: {name}", commandName);

            IParameters? parameters = commandRecord.ParameterParser.Parse(args);

            Log.Debug("Found params : {@params}", parameters);
            Log.Debug("Params Type : {T}", parameters?.GetType());
            
            switch (commandRecord.Delegate) {
                case Func<Task> func when parameters is null :
                    Log.Debug("Invoking async delegate without parameters.");
                    await func(); // For Async methods without parameters
                    return;
                
                case Action action when parameters is null:
                    Log.Debug("Invoking synchronous delegate without parameters.");
                    action(); // For non-async methods without parameters
                    return;
                
                case not null when commandRecord is { IsAsync: true }:
                    Log.Debug("Invoking async delegate with parameters.");
                    var task = (Task)commandRecord.Delegate.DynamicInvoke(parameters)!;
                    await task;
                    return;
                
                case not null :
                    Log.Debug("Invoking synchronous delegate with parameters.");
                    commandRecord.Delegate.DynamicInvoke(parameters); // For non-async methods with parameters
                    return;
                
                
                default:
                    throw new ConstraintException("Delegate could not be invoked");
            }
        }
        
        catch (Exception e) {
            Log.Error(e, "Error occurred during command execution.");
        }
    }
}