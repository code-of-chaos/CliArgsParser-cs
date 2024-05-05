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
public interface IParser {
    /// <summary>
    /// Tries to parse the input string.
    /// </summary>
    /// <param name="input">The input string to parse.</param>
    /// <remarks>
    /// This method attempts to parse the input string and perform any necessary operations based on the parsed data.
    /// </remarks>
    public void TryParse(string input);

    /// <summary>
    /// Asynchronously attempts to parse the given input.
    /// </summary>
    /// <param name="input">The input string to parse.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task TryParseAsync(string input);
}