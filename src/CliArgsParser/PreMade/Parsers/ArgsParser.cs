// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace CliArgsParser.PreMade.Parsers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ArgsParser(bool breakOnError = true) : AbstractParser(breakOnError) {
    public override void TryParse(string input) {
        Log.Debug("Starting command parsing.");

        GetCommands(input).ToList().ForEach(ProcessCommandString);
        
        Log.Debug("Finished command parsing."); ;
    }

    public override async Task TryParseAsync(string input) {
        Log.Debug("Starting async command parsing.");

        await Task.WhenAll(GetCommands(input).Select(ProcessCommandStringAsync));

        Log.Debug("Finished async command parsing."); ;
    }
}