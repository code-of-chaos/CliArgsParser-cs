// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CliArgsParser.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents an attribute that is used to mark a property as a boolean argument in command line argument parsing.
/// These types of attributes always return a bool.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ArgFlagAttribute(char shortName, string longName, string? description = null)
    : ArgAttribute(shortName, longName, description);