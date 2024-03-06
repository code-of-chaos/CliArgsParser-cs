// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CliArgsParser.Contracts.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents an argument attribute.
/// </summary>
public interface IArgAttribute {
    /// <summary>
    /// Gets the short name of the command line argument.
    /// </summary>
    public char ShortName { get; }

    /// <summary>
    /// Gets the long name of the argument attribute.
    /// </summary>
    public string LongName { get; }

    /// <summary>
    /// Interface for argument attributes used in command line argument parsing.
    /// </summary>
    public string? Description { get; }
}