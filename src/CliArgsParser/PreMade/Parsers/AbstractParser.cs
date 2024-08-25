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
/// <summary>
/// Abstract base class for parsers that parse command-line arguments.
/// </summary>
public abstract partial class AbstractParser(bool breakOnError) : IParser {
    /// <summary>
    /// Represents a logger used by the CliArgsParser library.
    /// </summary>
    protected ILogger Log { get; set; } = Logger.None;

    /// <summary>
    /// Dictionary that stores command records.
    /// </summary>
    private Dictionary<string, CommandRecord> CommandStructs { get; set; } = null!;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Ingests the setup information into the parser.
    /// </summary>
    /// <param name="setup">The setup information for the parser.</param>
    /// <returns>The instance of the parser.</returns>
    public IParser IngestFromSetup(ParserDto setup) {
        Log = setup.Logger;
        CommandStructs = setup.CommandStructs;

        if (setup.HasAsyncCommands) {
            Log.Information("{type} created & populated with Async Commands. Use `TryParseAsync` methods.", GetType().Name);
        }

        return this;
    }

    /// <summary>
    /// Retrieves the command line arguments from the specified input string.
    /// </summary>
    /// <param name="argsInput">The input string containing the command line arguments.</param>
    /// <returns>A dictionary representing the command line arguments, where the keys are the argument names and the values are the argument values.</returns>
    protected static Dictionary<string, string> GetArgs(string argsInput) {
        return ArgsRegex()
            .Matches(argsInput)
            .Where(match => match.Success)
            .ToDictionary(
                keySelector: match => match.Groups[1].Value,
                elementSelector: match => match.Groups[2].Success
                    ? match.Groups[2].Value.Trim('"')
                    : "True"
            );
    }

    /// <summary>
    /// Tries to get the command and its arguments from the input string.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <param name="commandName">When this method returns, contains the command name if it was successfully extracted; otherwise, contains null.</param>
    /// <param name="args">When this method returns, contains the command arguments if they were successfully extracted; otherwise, an empty dictionary.</param>
    /// <returns>true if the command and its arguments were successfully extracted from the input string; otherwise, false.</returns>
    protected static bool TryGetCommand(string input, [NotNullWhen(true)] out string? commandName, out Dictionary<string, string> args) {
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

    /// <summary>
    /// Splits the input into individual commands and trims them.
    /// </summary>
    /// <param name="input">The input string containing multiple commands.</param>
    /// <returns>An enumerable of individual commands.</returns>
    protected static IEnumerable<string> GetCommands(string input) {
        return SplitCommands()
            .Split(input)
            .Select(s => s.Trim());
    }

    /// <summary>
    /// Generates a regular expression for parsing command-line arguments.
    /// </summary>
    [GeneratedRegex("""(?:--|-)(\w+)(?:=(".*?"|\S+))?""")] 
    private static partial Regex ArgsRegex();

    /// <summary>
    /// Splits the input into individual commands and trims any leading or trailing whitespace.
    /// </summary>
    [GeneratedRegex("""&&(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)""")]
    private static partial Regex SplitCommands();

    /// <summary>
    /// Processes a command string by parsing the command, its arguments, and executing the corresponding delegate.
    /// </summary>
    /// <param name="commandString">The command string to process.</param>
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
            }
            else {
                commandRecord.Delegate.DynamicInvoke();
            }
        }

        catch (Exception e) {
            Log.Error(e, "Error occurred during command execution.");
            if (breakOnError) throw;
        }
    }

    /// <summary>
    /// Processes a command string asynchronously.
    /// </summary>
    /// <param name="commandString">The command string to be processed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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
                    await func();// For Async methods without parameters
                    return;

                case Action action when parameters is null or NoArgs:
                    Log.Debug("Invoking synchronous delegate without parameters.");
                    action();// For non-async methods without parameters
                    return;

                case {} del when commandRecord.IsAsync:
                    if (parameters == null) {
                        throw new ArgumentException("No parameters provided for async method that needs parameters");
                    }

                    Log.Debug("Invoking async delegate with parameters.");
                    var task = (Task)del.DynamicInvoke(parameters)!;
                    await task;
                    return;

                case {} del:
                    if (parameters == null) {
                        throw new ArgumentException("No parameters provided for method that needs parameters");
                    }

                    Log.Debug("Invoking synchronous delegate with parameters.");
                    del.DynamicInvoke(parameters);// For non-async methods with parameters
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
    /// <summary>
    /// Tries to parse the input string.
    /// </summary>
    /// <param name="input">The input string to be parsed.</param>
    public abstract void TryParse(string input);

    /// <summary>
    /// Tries to parse the given input asynchronously.
    /// </summary>
    /// <param name="input">The input string to parse.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public abstract Task TryParseAsync(string input);
}
