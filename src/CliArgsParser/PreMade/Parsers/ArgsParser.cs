// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace CliArgsParser.PreMade.Parsers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The `ArgsParser` class is responsible for parsing command-line arguments and executing commands based on the input.
/// </summary>
/// <remarks>
/// This class inherits from the `AbstractParser` class and implements the `IParser` interface.
/// </remarks>
public class ArgsParser(bool breakOnError = true) : AbstractParser(breakOnError) {
    /// <summary>
    /// Tries to parse the input string.
    /// </summary>
    /// <param name="input">The input string to parse.</param>
    public override void TryParse(string input) {
        Log.Debug("Starting command parsing.");

        GetCommands(input).ToList().ForEach(ProcessCommandString);
        
        Log.Debug("Finished command parsing.");
    }

    /// <summary>
    /// Asynchronously tries to parse the input string.
    /// </summary>
    /// <param name="input">The string to parse.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public override async Task TryParseAsync(string input) {
        Log.Debug("Starting async command parsing.");

        await Task.WhenAll(GetCommands(input).Select(ProcessCommandStringAsync));

        Log.Debug("Finished async command parsing."); 
    }
}