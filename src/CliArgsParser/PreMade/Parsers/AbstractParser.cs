// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using CliArgsParser.Contracts;
using CliArgsParser.Contracts.Common;
using Serilog;
using Serilog.Core;

namespace CliArgsParser.PreMade.Parsers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public abstract partial class AbstractParser : IParser{
    protected ILogger Log { get; set; } = Logger.None;
    protected Dictionary<string, CommandRecord> CommandStructs = null!;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    internal IParser IngestFromSetup(ParserDto setup) {
        Log = setup.Logger;
        CommandStructs = setup.CommandStructs;

        if (setup.HasAsyncCommands) {
            Log.Information("{type} created & populated with Async Commands. Use `TryParseAsync` methods.", GetType().Name);
        }
        
        return this;
    }

    public Dictionary<string, string> GetArgs(string argsInput) {
        return ArgsRegex()
            .Matches(argsInput)
            .Where(match => match.Success)
            .ToDictionary(
                match => match.Groups[1].Value,
                match => match.Groups[2].Success 
                    ? match.Groups[2].Value.Trim('"') 
                    : "True"
            );
    }

    public bool TryGetCommand(string input, [NotNullWhen(true)] out string? commandName, out Dictionary<string, string>? args) {
        commandName = null;
        args = default;
        
        string[] strings = input.Split(" ", 2);
        
        if (strings.Length < 1) {
            return false;
        }
        
        commandName = strings[0];
        args = strings.Length == 2 
            ? GetArgs(strings[1]) 
            : null;
        
        return true;
    }

    public IEnumerable<string> GetCommands(string input) {
        return SplitCommands()
            .Split(input)
            .Select(s => s.Trim());
    }
    
    [GeneratedRegex("""(--|-)(\w+)(=\"[^\"]*\"|\w*)?""")]
    public static partial Regex ArgsRegex();

    [GeneratedRegex("""&&(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)""")]
    public static partial Regex SplitCommands();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Abstract Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryParse(string input) => TryParse(input, out object? _);
    public async Task<bool> TryParseAsync(string input) => await TryParseAsync(input, out object? _);

    public abstract bool TryParse<T>(string input, out T? output) ;
    public abstract Task<bool> TryParseAsync<T>(string input, out T? output) ;

}