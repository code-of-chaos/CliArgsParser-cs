// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Contracts.Types;
using System.Collections.Immutable;

namespace CliArgsParser.Contracts;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICliArgsParser {
    ImmutableDictionary<Type, IParameterParser> parameterParsers { get; }
    CliArgsParserConfig Config { get; }
    ImmutableDictionary<string, CommandMethodInfo> Commands { get; }
    
    void Execute(string commandString);
    Task ExecuteAsync(string commandString);
}
