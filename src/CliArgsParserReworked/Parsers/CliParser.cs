// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParserReworked.Contracts;

namespace CliArgsParserReworked.Parsers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CliParser(ICliArgsParser cliArgsParser) : ICliParser {
    public bool IsAlive { get; set; } = true;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void StartParsing() {
        while (IsAlive) {
            Console.Write(cliArgsParser.Config.CliCursor);
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)) continue;
            cliArgsParser.Execute(input);
        }
    }
    
    public async Task StartParsingAsync() {
        while (IsAlive) {
            Console.Write(cliArgsParser.Config.CliCursor);
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)) continue;
            await cliArgsParser.ExecuteAsync(input);
        }
    }
}
