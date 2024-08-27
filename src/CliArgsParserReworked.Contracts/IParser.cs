// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CliArgsParserReworked.Contracts;
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
    public void TryParse(string input);
    public void TryParse(string[] input);

    /// <summary>
    /// Asynchronously attempts to parse the given input.
    /// </summary>
    /// <param name="input">The input string to parse.</param>
    public Task TryParseAsync(string input);
    public Task TryParseAsync(string[] input);
}
