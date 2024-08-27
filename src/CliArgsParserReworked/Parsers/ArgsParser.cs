// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParserReworked.Contracts;

namespace CliArgsParserReworked.Parsers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ArgsParser(ICliArgsParser cliArgsParser) : IArgsParser {
    public void Parse(string[] input) => Parse(string.Join(" ", input));
    public void Parse(string input) => cliArgsParser.Execute(input);
    
    public Task ParseAsync(string[] input) => ParseAsync(string.Join(" ", input));
    public Task ParseAsync(string input) => cliArgsParser.ExecuteAsync(input);
}
