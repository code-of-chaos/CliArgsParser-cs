// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CliArgsParser.Contracts.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents an attribute that decorates a method as a CLI command.
/// </summary>
public interface ICliCommandAttribute {
    /// <summary>
    /// Gets the name of the CLI command.
    /// </summary>
    string Name { get;}

    /// <summary>
    /// Represents an attribute that can be applied to a CLI command.
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// Represents the type of parameter options used by a CLI command.
    /// </summary>
    public Type ParameterOptionsType { get; }

    /// <summary>
    /// Retrieves the parameter options for a CLI command.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    /// <returns>The parameter options parsed from the command line arguments.</returns>
    IParameterOptions GetParameters(IEnumerable<string> args);
}