// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Contracts;
using CliArgsParser.PreMade;
using Microsoft.Extensions.DependencyInjection;

namespace CliArgsParser.Parsers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ArgsParser(ICliArgsParser cliArgsParser) : IArgsParser {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region Parsing
    public void Parse(string[] input) => Parse(string.Join(" ", input));
    public void Parse(string input) {
        foreach (string commandString in RegexLib.SplitCommands.Split(input).Select(c => c.Trim())) {
            cliArgsParser.Execute(commandString);
        }
    }

    public Task ParseAsync(string[] input) => ParseAsync(string.Join(" ", input));
    public async Task ParseAsync(string input) {
        foreach (string commandString in RegexLib.SplitCommands.Split(input).Select(c => c.Trim())) {
            await cliArgsParser.ExecuteAsync(commandString);
        }
    }
    #endregion

    #region Standalone implementation
    public static IArgsParser CreateStandalone(Action<ICliArgsParserConfiguration> action) {
        ServiceProvider provider = new ServiceCollection()
            .AddArgsParser(action)
            .BuildServiceProvider();
        
        return provider.GetRequiredService<IArgsParser>();
    }
    #endregion
}
