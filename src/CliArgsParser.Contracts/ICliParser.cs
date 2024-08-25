// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace CliArgsParser.Contracts;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

/// <summary>
/// Represents a parser that can parse input strings.
/// </summary>
public interface ICliParser : IParser {
    /// <summary>
    /// Parses continuous input from the command line and tries to parse each command string.
    /// </summary>
    /// <remarks>
    /// This method reads input continuously from the command line, prompts the user for input, and tries to parse each command string.
    /// If the input is null or empty, the method continues to prompt for input.
    /// </remarks>
    public void TryParseContinuous();

    /// <summary>
    /// Parses continuous input from the command line and tries to parse each command string.
    /// </summary>
    /// <remarks>
    /// This method reads input continuously from the command line, prompts the user for input, and tries to parse each command string.
    /// If the input is null or empty, the method continues to prompt for input.
    /// </remarks>
    public Task TryParseContinuousAsync();
}
