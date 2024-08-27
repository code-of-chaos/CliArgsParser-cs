// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace CliArgsParser;
// ReSharper restore CheckNamespace

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc cref="ICliParser"/>
public class CliParser(ICliArgsParser cliArgsParser) : ICliParser {
    /// <inheritdoc cref="ICliParser.IsAlive"/>
    public bool IsAlive { get; set; } = true;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region Parsing
    /// <inheritdoc cref="ICliParser.StartParsing"/>
    public void StartParsing() {
        while (IsAlive) {
            Console.Write(cliArgsParser.Config.CliCursor);
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)) continue;

            foreach (string commandString in RegexLib.SplitCommands.Split(input).Select(c => c.Trim())) {
                try {
                    cliArgsParser.Execute(commandString);
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }

    /// <inheritdoc cref="ICliParser.StartParsingAsync"/>
    public async Task StartParsingAsync() {
        while (IsAlive) {
            Console.Write(cliArgsParser.Config.CliCursor);
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)) continue;

            foreach (string commandString in RegexLib.SplitCommands.Split(input).Select(c => c.Trim())) {
                try {
                    await cliArgsParser.ExecuteAsync(commandString);
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
    #endregion

    #region Standalone implementation
    /// <summary>
    /// Creates a standalone instance of the CLI parser, which is responsible for parsing command line arguments and executing the specified commands when the program is run as a terminal tool.
    /// </summary>
    /// <param name="action">An <see cref="Action{ICliArgsParserConfiguration}"/> that configures the <see cref="ICliArgsParser"/>.</param>
    /// <returns>An instance of the standalone CLI parser</returns>
    public static ICliParser CreateStandalone(Action<ICliArgsParserConfiguration> action) {
        ServiceProvider provider = new ServiceCollection()
            .AddCliParser(action)
            .BuildServiceProvider();

        return provider.GetRequiredService<ICliParser>();
    }
    #endregion
}
