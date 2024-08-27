// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace CliArgsParser.Contracts;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a class that parses arguments and extracts parameters based on a specific type.
/// </summary>
public interface ICommandParameterParser {
    /// <summary>
    /// Represents the type of parameters used by the <see cref="ICommandParameterParser"/> class.
    /// </summary>
    Type ParamsType { get; }
    
    /// <summary>
    /// Tries to parse the given arguments and returns a boolean value indicating if the parsing was successful.
    /// If the parsing is successful, it sets the 'parameters' parameter to the parsed parameters object.
    /// </summary>
    /// <param name="args">The dictionary of arguments to parse.</param>
    /// <param name="parameters">When this method returns, contains the parsed parameters object if the parsing is successful; otherwise, null.</param>
    /// <returns>true if the parsing is successful; otherwise, false.</returns>
    bool TryParse(Dictionary<string, string> args, [NotNullWhen(true)] out ICommandParameters? parameters);
}
