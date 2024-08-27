// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParserReworked.Contracts.Types;
using System.Collections.Immutable;

namespace CliArgsParserReworked.Contracts;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICliArgsParser {
    ImmutableDictionary<Type, IParameterParser> parameterParsers { get; }
    CliArgsParserConfig Config { get; }
    ImmutableDictionary<string, CommandMethodInfo> Commands { get; }
    
    Task ExecuteAsync(string commandString);
}
