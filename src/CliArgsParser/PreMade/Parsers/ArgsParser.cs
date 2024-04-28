// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

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

        var commands = GetCommands(input);
        
        IEnumerable<string> commandStrings = commands.ToList();
        Log.Debug("Found {i} in {string}", commandStrings.Count(), input);

        foreach (string commandString in commandStrings) {
            Log.Debug($"Processing command: {commandString}");
            
            if (!TryGetCommand(commandString, out string? commandName, out Dictionary<string, string>? args)) {
                Log.Warning($"Failed to get command for string: {commandString}");
                continue;
            }

            if (!CommandStructs.TryGetValue(commandName, out CommandRecord? commandRecord)) {
                Log.Warning($"No command record found for command name: {commandName}");
                continue;
            }

            try {
                
                Log.Debug("Parsing parameters for command: {name}",commandName);
                
                IParameters? parameters = commandRecord.ParameterParser.Parse(args);
                
                Log.Debug("Found params : {@params}", parameters);
                
                if (parameters?.GetType() == typeof(NoArgs)) {
                    Log.Debug($"Command: {commandName} has no parameters.");
                    if (commandRecord.Delegate is Func<Task> func) {
                        Log.Debug("Invoking async delegate without parameters.");
                        await func(); // For Async methods without parameters
                        continue;
                    }
                    if (commandRecord?.Delegate is Action action) {
                        Log.Debug("Invoking synchronous delegate without parameters.");
                        action(); // For non-async methods without parameters
                        continue;
                    }
                }
                
                Log.Debug("{@a}", commandRecord?.Delegate?.Method);
                if (commandRecord is { IsAsync: true }) {
                    Log.Debug("Invoking async delegate with parameters.");
                    var task = (Task)commandRecord.Delegate?.DynamicInvoke(parameters)!;
                    await task;
                    continue;
                }

                Log.Debug("Invoking synchronous delegate with parameters.");
                commandRecord?.Delegate?.DynamicInvoke(parameters); // For non-async methods with parameters
            }
            catch (Exception e) {
                Log.Error(e, "Error occurred during command execution.");
            }
        }
        
        Log.Debug("Finished async command parsing.");
        return true;
    }
}