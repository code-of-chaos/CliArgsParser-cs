// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using CliArgsParser.Contracts.Attributes;

namespace CliArgsParser.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents an attribute that is used to mark a property as a boolean argument in command line argument parsing.
/// These types of attributes always return a bool.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public class ArgFlagAttribute(string shortName, string longName, string? description = null)
    : ArgFlag(shortName, longName, description);