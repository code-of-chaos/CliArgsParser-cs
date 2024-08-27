// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace CliArgsParser.Parsers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CliParser(ICliArgsParser cliArgsParser) : ICliParser {
    public bool IsAlive { get; set; } = true;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region Parsing
    public void StartParsing() {
        while (IsAlive) {
            Console.Write(cliArgsParser.Config.CliCursor);
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)) continue;
            
            foreach (string commandString in RegexLib.SplitCommands.Split(input).Select(c => c.Trim())) {
                cliArgsParser.Execute(commandString);
            }
        }
    }
    
    public async Task StartParsingAsync() {
        while (IsAlive) {
            Console.Write(cliArgsParser.Config.CliCursor);
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)) continue;
            
            foreach (string commandString in RegexLib.SplitCommands.Split(input).Select(c => c.Trim())) {
                await cliArgsParser.ExecuteAsync(commandString);
            }
        }
    }
    #endregion

    #region Standalone implementation
    public static IArgsParser CreateStandalone(Action<ICliArgsParserConfiguration> action) {
        ServiceProvider provider = new ServiceCollection()
            .AddCliParser(action)
            .BuildServiceProvider();
        
        return provider.GetRequiredService<IArgsParser>();
    }
    #endregion
}
