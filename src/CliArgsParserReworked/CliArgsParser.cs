// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParserReworked.Contracts;
using CliArgsParserReworked.Contracts.Types;
using System.Collections.Immutable;

namespace CliArgsParserReworked;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CliArgsParser(
    ImmutableDictionary<Type, ParameterParser> parameterParsers,
    CliArgsParserConfig configuration,
    Dictionary<string, CommandMethodInfo> commands
) : ICliArgsParser {
    private ImmutableDictionary<Type, ParameterParser> _parameterParsers = parameterParsers;
    private readonly CliArgsParserConfig _config = configuration;
    private ImmutableDictionary<string, CommandMethodInfo> Commands = commands.ToImmutableDictionary();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Execute(string[] args) {
        var command = args[0];
        var arguments = args.Skip(1).ToArray();

        if (Commands.TryGetValue(command, out CommandMethodInfo commandMethodInfo)) {
            Console.WriteLine($"Command: {command}");
        }
    }
}
