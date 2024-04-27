// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace CliArgsParser.Contracts.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Base class for attribute classes used in command line argument parsing.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public abstract class ArgAttribute(string shortName, string longName, string? description) : Attribute, IArgAttribute {
    /// <summary>
    /// Represents a short name attribute used in command-line argument parsing.
    /// Usually a single letter or a combination of initials of the LongName command
    /// </summary>
    public string ShortName { get; } = shortName;

    /// <summary>
    /// Represents a long name attribute for command-line arguments.
    /// </summary>
    public string LongName { get; } = longName.Replace("-", "");

    /// <summary>
    /// Represents the description of a command line argument.
    /// </summary>
    public string? Description { get; } = description;
}