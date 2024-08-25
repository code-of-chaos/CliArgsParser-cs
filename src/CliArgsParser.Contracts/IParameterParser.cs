// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace CliArgsParser.Contracts;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents an interface for parsing command-line parameters.
/// </summary>
public interface IParameterParser {
    /// <summary>
    /// Represents a parameter parser.
    /// </summary>
    public Type ParamsType { get; }

    /// <summary>
    /// Parses the command line arguments and returns the parsed parameters.
    /// </summary>
    /// <param name="args">The command line arguments as key-value pairs.</param>
    /// <returns>The parsed parameters as an instance of <see cref="IParameters"/> or null if parsing fails.</returns>
    public IParameters? Parse(Dictionary<string, string> args);
}
