// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using CliArgsParser.Contracts;

namespace CliArgsParser.PreMade.Parsers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CliParser(bool breakOnError = true) : AbstractParser(breakOnError) {
    public void TryParseContinous() {
        while (true) {
            Console.WriteLine("> ");
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)) continue;
            TryParse(input);
        }
    }
    
    public override void TryParse(string input) {
        GetCommands(input).ToList().ForEach(ProcessCommandString);
    }

    public override async Task TryParseAsync(string input) {
        await Task.WhenAll(GetCommands(input).Select(ProcessCommandStringAsync));
    }
}