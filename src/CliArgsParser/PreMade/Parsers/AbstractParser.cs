// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using CliArgsParser.Contracts;
using CliArgsParser.Contracts.Common;
using CliArgsParser.PreMade.Args;
using Serilog;
using Serilog.Core;

namespace CliArgsParser.PreMade.Parsers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public abstract partial class AbstractParser(bool breakOnError) : IParser{
    protected ILogger Log { get; set; } = Logger.None;
    protected Dictionary<string, CommandRecord> CommandStructs = null!;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IParser IngestFromSetup(ParserDto setup) {
        Log = setup.Logger;
        CommandStructs = setup.CommandStructs;

        if (setup.HasAsyncCommands) {
            Log.Information("{type} created & populated with Async Commands. Use `TryParseAsync` methods.", GetType().Name);
        }
        
        return this;
    }

    protected Dictionary<string, string> GetArgs(string argsInput) {
        return ArgsRegex()
            .Matches(argsInput)
            .Where(match => match.Success)
            .ToDictionary(
                match => match.Groups[2].Value,
                match => match.Groups[3].Success 
                    ? match.Groups[3].Value.Trim('"') 
                    : "True"
            );
    }

    protected bool TryGetCommand(string input, [NotNullWhen(true)] out string? commandName, out Dictionary<string, string> args) {
        commandName = null;
        args = new Dictionary<string, string>();
        
        string[] strings = input.Split(" ", 2);
        
        if (strings.Length < 1) {
            return false;
        }
        
        commandName = strings[0];
        if (strings.Length == 2) args = GetArgs(strings[1]);
        
        return true;
    }

    protected IEnumerable<string> GetCommands(string input) {
        return SplitCommands()
            .Split(input)
            .Select(s => s.Trim());
    }
    
    [GeneratedRegex("""(--|-)(\w+)(?:=(\"[^\"]*\"|\w*))?""")]
    protected static partial Regex ArgsRegex();

    [GeneratedRegex("""&&(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)""")]
    protected static partial Regex SplitCommands();
    
    protected void ProcessCommandString(string commandString) {
        if (!TryGetCommand(commandString, out string? commandName, out Dictionary<string, string> args))
            return;

        if (!CommandStructs.TryGetValue(commandName, out CommandRecord? commandRecord))
            return;

        try {
            IParameters? parameters = commandRecord.ParameterParser.Parse(args);
            Log.Warning("{p}", parameters);
            Log.Warning("{@p}", parameters);

            if (parameters != null && parameters.GetType() != typeof(NoArgs)) {
                commandRecord.Delegate.DynamicInvoke(parameters);
            } else {
                commandRecord.Delegate.DynamicInvoke();
            }
        }
        
        catch (Exception e){
            Log.Error(e, "Error occurred during command execution.");
            if (breakOnError) throw;
        }
    }
    
    protected async Task ProcessCommandStringAsync(string commandString) {
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
                case Func<Task> func when parameters is null or NoArgs:
                    Log.Debug("Invoking async delegate without parameters.");
                    await func(); // For Async methods without parameters
                    return;

                case Action action when parameters is null or NoArgs:
                    Log.Debug("Invoking synchronous delegate without parameters.");
                    action(); // For non-async methods without parameters
                    return;

                case { } del when commandRecord.IsAsync:
                    if (parameters == null) {
                        throw new ArgumentNullException(nameof(parameters),"No parameters provided for async method that needs parameters");
                    }

                    Log.Debug("Invoking async delegate with parameters.");
                    var task = (Task)del.DynamicInvoke(parameters)!;
                    await task;
                    return;

                case { } del:
                    if (parameters == null) {
                        throw new ArgumentNullException(nameof(parameters),"No parameters provided for method that needs parameters");
                    }

                    Log.Debug("Invoking synchronous delegate with parameters.");
                    del.DynamicInvoke(parameters); // For non-async methods with parameters
                    return;

                default:
                    throw new ConstraintException("Delegate could not be invoked");
            }
        }
        
        catch (Exception e) {
            Log.Error(e, "Error occurred during command execution.");
            if (breakOnError) throw;
        }
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Abstract Methods
    // -----------------------------------------------------------------------------------------------------------------
    public abstract void TryParse(string input) ;
    public abstract Task TryParseAsync(string input);

}