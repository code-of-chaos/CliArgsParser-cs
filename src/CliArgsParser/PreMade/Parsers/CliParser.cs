// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using CliArgsParser.Contracts;

namespace CliArgsParser.PreMade.Parsers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// CliParser is a class that implements the IParser interface and provides methods for parsing command-line input.
/// </summary>
public class CliParser(bool breakOnError = true) : AbstractParser(breakOnError), ICliParser {
    /// <summary>
    /// Parses continuous input from the command line and tries to parse each command string.
    /// </summary>
    /// <remarks>
    /// This method reads input continuously from the command line, prompts the user for input, and tries to parse each command string.
    /// If the input is null or empty, the method continues to prompt for input.
    /// </remarks>
    public void TryParseContinuous() {
        bool c = true;// this should be fixed
        while (c) {
            Console.Write("> ");
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)) continue;
            TryParse(input);
            c = true;
        }
    }

    /// <summary>
    /// Parses continuous input from the command line and tries to parse each command string.
    /// </summary>
    /// <remarks>
    /// This method reads input continuously from the command line, prompts the user for input, and tries to parse each command string.
    /// If the input is null or empty, the method continues to prompt for input.
    /// </remarks>
    public async Task TryParseContinuousAsync() {
        bool c = true;// this should be fixed
        while (c) {
            Console.Write("> ");
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)) continue;
            await TryParseAsync(input);
            c = true;
        }
    }

    /// <summary>
    /// Tries to parse the input string.
    /// </summary>
    /// <param name="input">The input string to parse.</param>
    public override void TryParse(string input) {
        GetCommands(input).ToList().ForEach(ProcessCommandString);
    }

    /// <summary>
    /// Tries to parse the input asynchronously.
    /// </summary>
    /// <param name="input">The input string to parse.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public override async Task TryParseAsync(string input) {
        await Task.WhenAll(GetCommands(input).Select(ProcessCommandStringAsync));
    }
}
