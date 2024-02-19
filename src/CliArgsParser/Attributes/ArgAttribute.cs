// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Contracts.Attributes;

namespace CliArgsParser.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Base class for attribute classes used in command line argument parsing.
/// </summary>
public abstract class ArgAttribute(char shortName, string longName, string? description) : Attribute, IArgAttribute {
    /// <summary>
    /// Represents a short name attribute used in command-line argument parsing.
    /// </summary>
    public char ShortName { get; } = shortName;

    /// <summary>
    /// Represents a long name attribute for command-line arguments.
    /// </summary>
    public string LongName { get; } = longName.Replace("-", "");

    /// <summary>
    /// Represents the description of a command line argument.
    /// </summary>
    public string? Description { get; } = description;
}