// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Immutable;

// ReSharper disable CheckNamespace
namespace CliArgsParser;
// ReSharper restore CheckNamespace

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a CLI Args Parser that parses command line arguments and executes corresponding commands.
/// </summary>
public interface ICliArgsParser {
    /// <summary>
    /// Holds the Config information defined during setup
    /// </summary>
    CliArgsParserConfig Config { get; }
    /// <summary>
    /// The collection of parameter parsers used by the CliArgsParser.
    /// Populated on first call.
    /// </summary>
    ImmutableDictionary<Type, ICommandParameterParser> ParameterParsers { get; }

    /// <summary>
    /// The collection of all registered commands available to the CliArgsParser.
    /// Populated on first call.
    /// </summary>
    ImmutableDictionary<string, CommandMethodInfo> Commands { get; }

    /// <summary>
    /// Executes a command specified by the given command string.
    /// </summary>
    /// <param name="commandString">The command string to parse and execute.</param>
    void Execute(string commandString);

    /// <summary>
    /// Executes a command specified by the given command string asynchronously.
    /// </summary>
    /// <param name="commandString">The command string to parse and execute.</param>
    Task ExecuteAsync(string commandString);
}
