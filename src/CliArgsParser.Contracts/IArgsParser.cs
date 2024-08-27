// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CliArgsParser.Contracts;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The ArgsParser class is responsible for parsing command line arguments and executing the specified commands,
/// when the program is run as a terminal tool.
/// </summary>
public interface IArgsParser {
    /// <summary>
    /// Parses the input string or string array into commands and executes them.
    /// </summary>
    /// <param name="input">The input string or string array.</param>
    void Parse(string input);
    /// <inheritdoc cref="Parse(string)"/>
    void Parse(string[] input);
    
    /// <summary>
    /// Asynchronously parses the given input string into commands and executes them.
    /// </summary>
    /// <param name="input">The input string to parse and execute.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ParseAsyncLinear(string input);
    /// <inheritdoc cref="ParseAsyncLinear(string)"/>
    Task ParseAsyncLinear(string[] input);
    
    /// <summary>
    /// Parses the given input string into commands and executes them in parallel.
    /// </summary>
    /// <param name="input">The input string to parse and execute.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ParseAsyncParallel(string input);
    /// <inheritdoc cref="ParseAsyncParallel(string)"/>
    Task ParseAsyncParallel(string[] input);
}
