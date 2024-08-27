// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace CliArgsParser.Parsers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc cref="IArgsParser"/>
public class ArgsParser(ICliArgsParser cliArgsParser) : IArgsParser {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region Parsing
    /// <inheritdoc cref="IArgsParser.Parse(string[])"/>
    public void Parse(string[] input) => Parse(string.Join(" ", input));
    /// <inheritdoc cref="IArgsParser.Parse(string)"/>
    public void Parse(string input) {
        foreach (string commandString in RegexLib.SplitCommands.Split(input).Select(c => c.Trim())) {
            cliArgsParser.Execute(commandString);
        }
    }

    /// <inheritdoc cref="IArgsParser.ParseAsyncLinear(string[])"/>
    public Task ParseAsyncLinear(string[] input) => ParseAsyncLinear(string.Join(" ", input));
    /// <inheritdoc cref="IArgsParser.ParseAsyncLinear(string)"/>
    public async Task ParseAsyncLinear(string input) {
        foreach (string commandString in RegexLib.SplitCommands.Split(input).Select(c => c.Trim())) {
            await cliArgsParser.ExecuteAsync(commandString);
        }
    }


    /// <inheritdoc cref="IArgsParser.ParseAsyncParallel(string[])"/>
    public Task ParseAsyncParallel(string[] input) => ParseAsyncParallel(string.Join(" ", input));
    /// <inheritdoc cref="IArgsParser.ParseAsyncParallel(string)"/>
    public async Task ParseAsyncParallel(string input) {
        IEnumerable<Task> enumerable = RegexLib.SplitCommands.Split(input).Select(c => c.Trim())
            .Select(cliArgsParser.ExecuteAsync);
        
        await Task.WhenAll(
            enumerable
        );
    }
    #endregion
    #region Standalone implementation
    /// <summary>
    /// Creates a standalone instance of <see cref="IArgsParser"/> with the specified configuration.
    /// </summary>
    /// <param name="action">An <see cref="Action{ICliArgsParserConfiguration}"/> that configures the <see cref="ICliArgsParser"/>.</param>
    /// <returns>A standalone instance of <see cref="IArgsParser"/>.</returns>
    public static IArgsParser CreateStandalone(Action<ICliArgsParserConfiguration> action) {
        ServiceProvider provider = new ServiceCollection()
            .AddArgsParser(action)
            .BuildServiceProvider();
        
        return provider.GetRequiredService<IArgsParser>();
    }
    #endregion
}
