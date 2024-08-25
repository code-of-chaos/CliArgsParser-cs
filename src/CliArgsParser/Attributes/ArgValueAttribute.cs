// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using CliArgsParser.Contracts.Attributes;

namespace CliArgsParser.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents an attribute that can be applied to properties in a class to define command line argument values.
/// These attributes always return string, or from string castable values
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ArgValueAttribute(string shortName, string longName, string? description = null)
    : ArgValue(shortName, longName, description);
